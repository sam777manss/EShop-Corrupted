using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AuthenticateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }
        public async Task<IActionResult> LogOutPageAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7257");
                var response = await client.GetAsync("/Authenticate/LogOut");
                var responseContent = await response.Content.ReadAsStringAsync();
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "User");
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginPage(Login login)
        {
            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7257/Authenticate/Login");
                    var response = await client.PostAsJsonAsync("", login);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        CommonIndex? Data = JsonConvert.DeserializeObject<CommonIndex>(responseContent);
                        if(Data.User != null && Data.Roles != null)
                        {
                            if (Data.Roles.Contains("Admin"))
                            {
                                return RedirectToAction("Index", "Admin", Data.Admin);
                            }
                            else
                            {
                                return RedirectToAction("Index", "User", Data.User);
                            }
                        }
                        else if(Data.response.Message == "Invalid Email")
                        {
                            ModelState.AddModelError("Email", "Email not found");
                        }
                        else if(Data.response.Message == "Invalid Password")
                        {
                            ModelState.AddModelError("Password", "password not valid");
                        }
                        //return View(responseContent);
                    }
                }
            }
            return View(login);
        }
        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPage(Register register)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("https://localhost:7257/Authenticate/Register");
                    client.BaseAddress = new Uri("https://localhost:7257/Authenticate/RegisterAdmin");
                    var response = await client.PostAsJsonAsync("", register);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        //return View(responseContent);
                        return RedirectToAction("LoginPage", "Authenticate");
                    }
                }
            }
            return View(register);
        }
    }
}
