using SalesGraph.Core.DataAccess.Models.Entities;
using System.ComponentModel.DataAnnotations;


namespace SalesGraph.ViewModels.Parameters
{
    public class AddSaleItem
    {
        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public virtual SaleItem ToEntity()
        {
            return new SaleItem()
            {
                SaleDate = SaleDate,
                Quantity = Quantity,
                ProductId = ProductId
            };
        }
    }
}
