using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesApi.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalLoginController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public ExternalLoginController(SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        [HttpGet("ExternalLoginFB")]
        public IActionResult ExternalLoginFB()
        {
            var provider = "Facebook";
            //var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "ExternalLoginController", null, Request.Scheme);
            var redirectUrl = "api/ExternalLogin/ExternalLoginCallback";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        
        }
        [AllowAnonymous]
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle login failure
                return BadRequest("External login failed.");
            }
            // cookie will be created by below code
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // Access the user's information from the cookie
                var user = HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = user.Identity?.Name;

                return Redirect("https://localhost:7109/User/Index");
            }
            else
            {
                AppUser new_user = new AppUser
                {

                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                IdentityResult identResult = await _userManager.CreateAsync(new_user);

                if (identResult.Succeeded)
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                    identResult = await _userManager.AddLoginAsync(new_user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(new_user, false);
                        return Redirect("https://localhost:7109/User/Index");
                    }
                }
                // User is not registered, handle registration or show an error message
            }
            return NotFound("User not found.");
        }

        [AllowAnonymous]
        [HttpGet("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "ExternalLogin");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };

                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };
            return NotFound("User not found.");
            
        }
    }
}
