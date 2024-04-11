using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.ViewModels.Parameters;

namespace SalesGraph.ViewModels.Results
{
    public class SaleList
    {
        public List<EditSaleItem> Sales { get; private set; }
        
        public int TotalPages { get; set; }
        
        public SaleList ToResult(IEnumerable<SaleItem> sales, int totalPages)
        {
            Sales = sales.Select(s => new EditSaleItem()
            {
                Id = s.SaleItemId,
                SaleDate = s.SaleDate,
                Quantity = s.Quantity,
                ProductId = s.ProductId
            }).ToList();
            TotalPages = totalPages;
        
            return this;
        }
    }
}
