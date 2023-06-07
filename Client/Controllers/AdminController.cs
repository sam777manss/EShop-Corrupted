using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Security.Claims;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        public static string URL = "https://localhost:7257/";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //AdminIndex adminIndex = new AdminIndex();
            return View();
        }
        #region fetch all the users 
        public async Task<IActionResult> AdminTables()
        {
            try
            {
                var user = HttpContext.User;

                // Access the desired claims by their claim type
                var Uid = user.FindFirst(ClaimTypes.PrimarySid)?.Value;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var response = await client.GetAsync("Admin/AdminTables?Uid=" + Uid); // using concatenation
                    var responseContent = await response.Content.ReadAsStringAsync();
                    List<AdminIndex> users = JsonConvert.DeserializeObject<List<AdminIndex>>(responseContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return View(users);
                    }
                }
                return View(new List<AdminIndex>()); // Return an empty list if the user is not an admin or an error occurs
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);

            }
            return View(new List<AdminIndex>());
        }
        #endregion

        #region fetch user data using id and return data to partial view
        [HttpGet]
        public async Task<ActionResult> Edit(string Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                var response = await client.GetAsync("Admin/Edit?Id=" + Id); // using concatenation
                var responseContent = await response.Content.ReadAsStringAsync();
                AppUser user = JsonConvert.DeserializeObject<AppUser>(responseContent);
                if (response.IsSuccessStatusCode)
                {
                    return PartialView("_UserModal", user);
                }
            }
            return PartialView("_UserModal", new AppUser());
        }
        #endregion

        #region update user details and profile photo
        [HttpPost]
        public async Task<ActionResult> SaveEdits([FromForm] AppUser appUser, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid) // true when all values included or when user images exits
                {
                    using (var client = new HttpClient())
                    {
                        // Send the image name to web api
                        appUser.imageUrl = appUser.imageFile.FileName;
                        client.BaseAddress = new Uri(URL + "Admin/SaveEdits");
                        var response = await client.PostAsJsonAsync("", appUser);
                        // Handle the response from the Web API controller
                        if (response.IsSuccessStatusCode)
                        {
                            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagesFolder", appUser.imageFile.FileName);
                            // Save the image to the specified path
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                appUser.imageFile.CopyTo(stream);
                            }
                            return PartialView("_UserModal", appUser);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to upload the image");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);

            }
            return PartialView("_UserModal", appUser);
        }
        #endregion

        #region Delete user using id
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var response = await client.DeleteAsync("Admin/Delete?Id=" + Id);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        // TempData flow bidirectional but ViewBag and ViewData flow from controller to view only
                        TempData["DeletionMessage"] = "Deletion completed successfully";
                        return RedirectToAction("AdminTables");
                    }
                }
            }
            return View();
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> AdminAccount()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] AddProduct addProduct)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Set the base address of the Web API endpoint
                    client.BaseAddress = new Uri(URL + "Admin/AddProduct");

                    // Create a new instance of MultipartFormDataContent
                    var multiContent = new MultipartFormDataContent();

                    // Add the fields from the AddProduct object
                    multiContent.Add(new StringContent(addProduct.ProductName ?? ""), "ProductName");
                    multiContent.Add(new StringContent(addProduct.ProductDescription ?? ""), "ProductDescription");
                    multiContent.Add(new StringContent(addProduct.ProductType ?? ""), "ProductType");
                    multiContent.Add(new StringContent(addProduct.ProductCategory ?? ""), "ProductCategory");
                    multiContent.Add(new StringContent(addProduct.ProductCategoryType ?? ""), "ProductCategoryType");
                    multiContent.Add(new StringContent(addProduct.ProductCategoryDescription ?? ""), "ProductCategoryDescription");
                    multiContent.Add(new StringContent(addProduct.VendorEmail ?? ""), "VendorEmail");

                    // Convert the AddProduct object to JSON and add it as a StringContent
                    var addProductJson = JsonConvert.SerializeObject(addProduct);
                    multiContent.Add(new StringContent(addProductJson), "addProduct");

                    // Add the image files
                    foreach (var file in addProduct.Files)
                    {
                        var fileContent = new StreamContent(file.OpenReadStream());
                        multiContent.Add(fileContent, "Files", file.FileName);
                    }

                    // Send the HTTP POST request to the Web API
                    var response = await client.PostAsync("", multiContent);

                    // Add the image files to the wwwroot folder
                    foreach (var file in addProduct.Files)
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagesFolder", file.FileName);
                        // Save the image to the specified path
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", "Failed to upload the image");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to upload the image");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return View();
        }
    }
}
