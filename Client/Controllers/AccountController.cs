using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    #region account controller is for like unauthorise access
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Login failed when user try to load (URL change from header) Admin page without login 
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return RedirectToAction("LoginPage", "Authenticate");
        }
        #endregion

        #region AccessDenied when logged in user try to access non-authorize page
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
    #endregion
}
