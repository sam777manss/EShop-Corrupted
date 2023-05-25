using Microsoft.AspNetCore.Mvc;

namespace ShoesApi.Controllers
{
    public class AdminDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
