using Microsoft.AspNetCore.Authorization;
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

        public ExternalLoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "ExternalLoginController", null, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        [HttpGet("signin-facebook")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle login failure
                return BadRequest("External login failed.");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // User is successfully authenticated
                return Ok();
            }
            else
            {
                // User is not registered, handle registration or show an error message
                return NotFound("User not found.");
            }
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
