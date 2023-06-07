using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesApi.DbContextFile.DBFiles
{
    public partial class AddProductTable
    {
        [Key]
        public Guid? ProductId { get; set;}
        public Guid? GroupId { get; set;}
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductType { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductCategoryDescription { get; set; }
        public string? ProductCategoryType { get; set; }
        //public string? ProductCategoryName { get; set; }
        [Required(ErrorMessage = "Please select files")]
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
        public string? VendorEmail { get; set; }
        public string? ImageUrl { get; set; }
    }
}
