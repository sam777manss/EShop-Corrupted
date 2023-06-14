using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoesApi.DbContextFile.DBFiles
{
    public class ProductImageTable
    {
        [Key]
        public Guid? ProductImgId { get; set; }
        public Guid? ProductImgGroupId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual AddProductTable AddProductTables { get; set; }
    }
}
