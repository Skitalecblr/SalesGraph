using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesGraph.Core.DataAccess.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public List<SaleItem> Sales { get; set; }

        [NotMapped]
        public int TotalSales { get; set; }
    }
}
