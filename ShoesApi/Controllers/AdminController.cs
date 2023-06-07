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

        #region fetch all user info
        [HttpGet]
        [Route("AdminTables")]
        public async Task<IActionResult> AdminTables(string Uid)
        {
            List<AdminIndex> adminIndexes = new List<AdminIndex>();
            adminIndexes = await admin.AdminTables(Uid);
            return Ok(adminIndexes);
        }
        #endregion

        #region fetch user data using id
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string Id)
        {
            AppUser adminIndexes = new AppUser();
            adminIndexes = await admin.Edit(Id);
            return Ok(adminIndexes);
        }
        #endregion

        #region Delete a user using id
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id != null)
            {
                bool flag = await admin.Delete(Id);
                if (flag)
                {
                    return new StatusCodeResult(204); // Deletion completed but return void status
                }
            }
            return new StatusCodeResult(500); // The request was not completed. The server met an unexpected condition.
        }
        #endregion

        #region update user data
        [HttpPost]
        [Route("SaveEdits")]
        public async Task<IActionResult> SaveEdits(AppUser appUser)
        {
            IActionResult result = await admin.SaveEdits(appUser);
            // Return a response indicating success
            return new StatusCodeResult(200);
        }
        #endregion
        #region update user data
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProduct product)
        {
            IActionResult result = await admin.AddProduct(product);
            // Return a response indicating success
            return new StatusCodeResult(200);
        }
        #endregion
    }
}
