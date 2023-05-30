using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        public static string URL = "https://localhost:7257/";

        [Authorize(Roles = "Admin")]
        public IActionResult Index(AdminIndex adminIndex)
        {
            //AdminIndex adminIndex = new AdminIndex();
            return View(adminIndex);
        }
        #region AdminTables fetch all the users 
        public async Task<IActionResult> AdminTables()
        {
            var user = HttpContext.User;

            // Access the desired claims by their claim type
            var Uid = user.FindFirst(ClaimTypes.PrimarySid)?.Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7257");
                //var response = await client.GetAsync($"/AdminData/AdminTables?Uid={Uid}"); // Using string interpolation
                var response = await client.GetAsync("/AdminData/AdminTables?Uid="+ Uid); // using concatenation
                var responseContent = await response.Content.ReadAsStringAsync();
                List<AdminIndex> users = JsonConvert.DeserializeObject<List<AdminIndex>>(responseContent);
                if(response.IsSuccessStatusCode)
                {
                    return View(users);
                }    
            }
            return View(new List<AdminIndex>()); // Return an empty list if the user is not an admin or an error occurs

        }
        #endregion
    }
}
