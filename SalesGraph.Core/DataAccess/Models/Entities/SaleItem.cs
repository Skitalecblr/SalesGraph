using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesGraph.Core.DataAccess.Models.Entities
{
    public class SaleItem
    {
        [Key]
        public Guid SaleItemId { get; set; }

        [Required(ErrorMessage = "Sales date is required")]
        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [ForeignKey("Product")]
        [Required(ErrorMessage = "Product is required")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
