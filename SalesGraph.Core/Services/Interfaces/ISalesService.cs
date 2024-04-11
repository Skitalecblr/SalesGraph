using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.Core.DataAccess.Models.Infrastructure;
using static SalesGraph.Core.Infrastructure.Enums.Enums;

namespace SalesGraph.Core.Services.Interfaces
{
    public interface ISalesService
    {
        Task<SaleItem> CreateSaleItem(SaleItem saleItem);

        Task<SaleItem> UpdateSaleItem(SaleItem saleItem);

        Task<bool> DeleteSaleItem(Guid id);

        Task<int> GetSaleItemsPagesCount(Guid productId, int pageSize);

        Task<List<SaleItem>> GetSaleItemsByProductId(Guid productId, int pageSize, int pageNumber, int totalPages);

        Task<GraphResult> GetSalesGroupedByTimePeriod(DateTime startDate, DateTime endDate,
            GroupingParam groupingParam);
    }
}
