using Client.Models;
using Microsoft.AspNetCore.Mvc;


namespace Client.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index(AdminIndex adminIndex)
        {
            //AdminIndex adminIndex = new AdminIndex();
            return View(adminIndex);
        }
        public IActionResult AdminTables()
        {
            return View();
        }
    }
}
