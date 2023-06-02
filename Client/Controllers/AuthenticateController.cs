using Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Security.Claims;

namespace Client.Controllers
{
    public class AuthenticateController : Controller
    {
        public static string URL = "https://localhost:7257/";
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }
        public async Task<IActionResult> LogOutPage()
        {
            // Retrieve the current user's claims from the HttpContext
            var user = HttpContext.User;

            // Access the desired claims by their claim type
            var passwordClaim = user.FindFirst(ClaimTypes.PrimarySid)?.Value;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "User");
        }

        public async Task CookiesSetUp(string UserId, List<string> roles, string username, string email)
        {
            try
            {
                // cookies set start
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, UserId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, username),
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(10), // user authentication timeout
                };
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);

            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginPage(Login login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL + "Authenticate/Login");
                    var response = await client.PostAsJsonAsync("", login);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        CommonIndex? Data = JsonConvert.DeserializeObject<CommonIndex>(responseContent);
                        if (Data?.UserId != null)
                        {
                            var userRoles = new List<string>();
                            var user_name = string.Empty;
                            var email = string.Empty;

                            if (Data.Roles.Contains("Admin"))
                            {
                                userRoles.Add("Admin");
                                userRoles.Add("User");
                                user_name = Data?.Admin?.Name;
                                email = Data?.Admin?.Email;
                            }
                            else
                            {
                                userRoles.Add("User");
                                user_name = Data?.User?.Name;
                                email = Data?.User?.Email;
                            }

                            await CookiesSetUp(Data.UserId, userRoles, user_name, email);
                            // cookies set ends
                            if (Data.User != null && Data.Roles != null)
                            {
                                if (Data.Roles.Contains("Admin"))
                                {
                                    return RedirectToAction("Index", "User");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "User"); // no need to pass values extract userdata in user index method
                                    //return RedirectToAction("Index", "User", Data.User);
                                }
                            }
                            else if (Data?.response?.Message == "Invalid Email")
                            {
                                ModelState.AddModelError("Email", "Email not found");
                            }
                            else if (Data?.response?.Message == "Invalid Password")
                            {
                                ModelState.AddModelError("Password", "password not valid");
                            }
                        }
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
                    client.BaseAddress = new Uri(URL + "Authenticate/RegisterAdmin");
                    var response = await client.PostAsJsonAsync("", register);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("LoginPage", "Authenticate");
                    }
                }
            }
            return View(register);
        }
    }
}
