﻿using Microsoft.AspNetCore.Mvc;
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
                return Ok();
            }
            return Unauthorized();
        }
    }
}
