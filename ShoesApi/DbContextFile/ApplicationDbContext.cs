using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoesApi.DbContextFile.DBFiles;
using ShoesApi.Models;

namespace ShoesApi.DbContextFile
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<AddProductTable> AddProductTable { get; set; }
        public virtual DbSet<ProductImageTable> ProductImageTable { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
