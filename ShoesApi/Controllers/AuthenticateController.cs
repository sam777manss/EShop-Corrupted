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
        public IActionResult Index()
        {
            return Ok();
        }
        public async Task<IActionResult> Register(Register register)
        {
            if(ModelState.IsValid)
            {
                bool successful = await _user.RegisterUser(register);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
