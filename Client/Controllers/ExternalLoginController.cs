using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ExternalLoginController : Controller
    {
        public static string URL = "https://localhost:7257/";
        public IActionResult Index(string content)
        {
            return View((object)content);
        }


        public async Task<IActionResult> GoogleLoginAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                var response = await client.GetAsync("api/ExternalLogin/GoogleLogin");
                var responseContent = await response.Content.ReadAsStringAsync();
                return View("Index", responseContent);
            }
        }


    }
}
