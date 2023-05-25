using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(UserIndex userIndex)
        {
            return View(userIndex);
        }
    }
}
