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
        public IActionResult Index(AdminIndex adminIndex)
        {
            //AdminIndex adminIndex = new AdminIndex();
            return View(adminIndex);
        }
        #region AdminTables fetch all the users 
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

        [HttpPost]
        public async Task<ActionResult> SaveEdits([FromForm] AppUser appUser, IFormFile imageFile) 
        {
            try
            {
                if (ModelState.IsValid) // true when all values included or when user images exits
                {
                    // Create a HttpClient to send the image to the Web API controller
                    using (var client = new HttpClient())
                    {
                        // set the path wwwroot/ImagesFolder
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagesFolder", appUser.imageFile.FileName);
                        // Send the image path to the Web API controller
                        appUser.imageUrl = appUser.imageFile.FileName;
                        client.BaseAddress = new Uri(URL + "Admin/SaveEdits");
                        var response = await client.PostAsJsonAsync("", appUser);
                        // Handle the response from the Web API controller
                        if (response.IsSuccessStatusCode)
                        {
                            // Save the image to the specified path
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                appUser.imageFile.CopyTo(stream);
                            }
                            // Image uploaded successfully
                            return PartialView("_UserModal", appUser);
                        }
                        else
                        {
                            // Failed to upload the image
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
                    if(response.IsSuccessStatusCode)
                    {
                        TempData["DeletionMessage"] = "Deletion completed successfully"; 
                        return RedirectToAction("AdminTables");
                        //return View(response);
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AdminAccount()
        {
            return View();
        }
    }
}
