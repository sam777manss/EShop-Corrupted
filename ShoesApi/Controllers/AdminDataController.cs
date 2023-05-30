using Microsoft.AspNetCore.Mvc;
using ShoesApi.Interfaces;
using ShoesApi.Models;
using System.Security.Claims;

namespace ShoesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminDataController : ControllerBase
    {
        private readonly IAdmin admin; 
        public AdminDataController(IAdmin admin) {
            this.admin = admin;
        }

        [HttpGet]
        [Route("AdminTables")]
        public async Task<IActionResult> AdminTables(string Uid)
        {
            List<AdminIndex> adminIndexes = new List<AdminIndex>();
            adminIndexes = await admin.AdminTables(Uid);
            return Ok(adminIndexes);
        }
    }
}
