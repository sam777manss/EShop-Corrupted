using Microsoft.AspNetCore.Mvc;
using ShoesApi.Interfaces;
using ShoesApi.Models;

namespace ShoesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin admin;
        public AdminController(IAdmin admin)
        {
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
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string Id)
        {
            UserIndex adminIndexes = new UserIndex();
            adminIndexes = await admin.Edit(Id);
            return Ok(adminIndexes);
        }

        #region Delete a user
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            if(Id != null)
            {
                bool flag = await admin.Delete(Id);
                if(flag)
                {
                    return new StatusCodeResult(204); // Deletion completed but return void status
                }
            }
            return new StatusCodeResult(500); // The request was not completed. The server met an unexpected condition.
        }
        #endregion
    }
}
