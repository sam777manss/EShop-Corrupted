using Microsoft.AspNetCore.Mvc;
using ShoesApi.Interfaces;
using ShoesApi.Models;

namespace ShoesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUser _user;
        // Need To be included in constructor all the interfaces

        public AuthenticateController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Register register)
        {
            if(ModelState.IsValid)
            {
                bool successful = await _user.RegisterUser(register);
                if (successful)
                {
                    return Ok();
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AdminIndex successful = await _user.LoginUser(login);
                if (successful.Email != null)
                {
                    return Ok(successful);
                }
            }
            return Unauthorized();
        }
    }
}
