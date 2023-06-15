using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoesApi.DbContextFile.DBFiles;
using ShoesApi.Models;
using System.Net;
using System.Reflection.Emit;

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
            builder.Entity<ProductImageTable>()
                .HasOne(e => e.AddProductTables)
                .WithMany(e => e.ProductImageTable)
                .HasForeignKey(e => e.ProductImgId).IsRequired();

            builder.Entity<ProductImageTable>()
                .HasKey(d => d.ProductImgGroupId);

            builder.Entity<AddProductTable>()
                .HasMany(e => e.ProductImageTable)
                .WithOne(z => z.AddProductTables)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AddProductTable>()
                .HasKey(d => d.ProductId);

            base.OnModelCreating(builder);
        }
    }
}
