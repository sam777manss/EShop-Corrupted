using Client.Models;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public IActionResult LoginPage(Login login)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Index", "Admin");
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
                    client.BaseAddress = new Uri("https://localhost:7257/Authenticate/Register");
                    var response = await client.PostAsJsonAsync("", register);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return View(responseContent);
                    }
                }
            }
            return View(register);
        }
    }
}
