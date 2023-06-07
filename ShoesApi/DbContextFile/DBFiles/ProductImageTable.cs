using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoesApi.DbContextFile.DBFiles
{
    public class ProductImageTable
    {
        [Key]
        public Guid? ProductId { get; set; }
        public Guid? GroupId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
