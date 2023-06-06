using Microsoft.AspNetCore.Mvc;
using ShoesApi.DbContextFile.DBFiles;
using ShoesApi.Models;

namespace ShoesApi.Interfaces
{
    public interface IAdmin
    {
        public Task<List<AdminIndex>> AdminTables(string Uid);

        public Task<bool> Delete(string Id);

        public Task<AppUser> Edit(string Id);

        public Task<IActionResult> SaveEdits(AppUser User);

        public Task<IActionResult> AddProduct(AddProduct product);
    }
}
