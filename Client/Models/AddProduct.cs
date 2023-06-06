using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class AddProduct
    {
        public string? ProductName { get; set;}
        public string? ProductDescription { get; set;}
        public string? ProductType { get; set;}
        public string? ProductCategory { get; set;}
        public string? ProductCategoryDescription { get; set;}
        public string? ProductCategoryType { get; set;}
        public string? ProductCategoryName { get; set;}
        [Required(ErrorMessage = "Please select files")]
        public List<IFormFile>? Files { get; set; }
        public string? VendorEmail { get; set; }
    }
}
