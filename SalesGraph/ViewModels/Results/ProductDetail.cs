using SalesGraph.Core.DataAccess.Models.Entities;

namespace SalesGraph.ViewModels.Results
{
    public class ProductDetail
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public int TotalSales { get; set; }

        public ProductDetail ToResult(Product product)
        {
            Id = product.ProductId;
            Name = product.Name;
            Cost = product.Cost;
            TotalSales = product.TotalSales;

            return this;
        }
    }
}
