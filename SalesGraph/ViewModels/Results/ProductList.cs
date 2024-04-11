using SalesGraph.Core.DataAccess.Models.Entities;

namespace SalesGraph.ViewModels.Results
{
    public class ProductListItem
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }
    }
    
    public class ProductList
    {
        public ProductList()
        {
            Products = new List<ProductListItem>();
        }

        public List<ProductListItem> Products { get; private set; }

        public ProductList ToResult(IEnumerable<Product> products)
        {
            Products = products.Select(p => new ProductListItem()
            {
                ProductId = p.ProductId,
                Name = p.Name,
            }).ToList();

            return this;
        }
    }
}
