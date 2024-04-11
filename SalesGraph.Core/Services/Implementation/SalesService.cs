using Microsoft.EntityFrameworkCore;
using SalesGraph.Core.DataAccess.Context;
using Microsoft.Extensions.Logging;
using SalesGraph.Core.Services.Interfaces;
using static SalesGraph.Core.Infrastructure.Enums.Enums;
using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.Core.DataAccess.Models.Infrastructure;

namespace SalesGraph.Core.Services.Implementation
{
    public class SalesService : ISalesService
    {
        private readonly SalesGraphContext _context;
        private readonly ILogger<ProductService> _logger;

        public SalesService(SalesGraphContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SaleItem> CreateSaleItem(SaleItem saleItem)
        {
            try
            {
                saleItem.SaleItemId = Guid.NewGuid();
                _context.Sales.Add(saleItem);
                await _context.SaveChangesAsync();
                return saleItem;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while adding the sale item to the database.");
                throw new Exception("An error occurred while adding the sale item to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the sale.");
                throw new Exception("An unexpected error occurred while adding the sale.");
            }
        }

        // Update
        public async Task<SaleItem> UpdateSaleItem(SaleItem saleItem)
        {
            try
            {
                _context.Entry(saleItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return saleItem;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception("Concurrency error occurred while updating the sale", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the sale", ex);
            }
        }

        public async Task<bool> DeleteSaleItem(Guid id)
        {
            try
            {
                var saleItem = await _context.Sales.FindAsync(id);
                if (saleItem == null)
                    return false;

                _context.Sales.Remove(saleItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the sale.");
                throw new Exception("An error occurred while deleting the sale.");
            }
        }

        //Get pages count
        public async Task<int> GetSaleItemsPagesCount(Guid productId, int pageSize)
        {
            int totalSaleItems = await _context.Sales.CountAsync(s => s.ProductId == productId);

            return (int)Math.Ceiling((double)totalSaleItems / pageSize);
        }

        //Get with pagination
        public async Task<List<SaleItem>> GetSaleItemsByProductId(Guid productId, int pageSize, int pageNumber, int totalPages)
        {
            try
            {
                pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));
                int itemsToSkip = (pageNumber - 1) * pageSize;
                var saleItems = await _context.Sales
                    .Where(s => s.ProductId == productId)
                    .OrderByDescending(s => s.SaleDate)
                    .Skip(itemsToSkip)
                    .Take(pageSize)
                    .ToListAsync();

                return saleItems;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales.", ex);
            }
        }

        public async Task<GraphResult> GetSalesGroupedByTimePeriod(DateTime startDate, DateTime endDate, GroupingParam groupingParam)
        {
            var groupedSales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .GroupBy(s => _context.GetGroupingKey(s.SaleDate, (int)groupingParam))
                .OrderBy(g => g.First().SaleDate)
                .Select(group => new GroupedSaleItem
                {
                    PeriodStartDate = group.Key,
                    TotalSaleCount = group.Count(),
                    TotalSalesSum = group.Sum(s => s.Quantity * s.Product.Cost) / 1000
                })
                .ToListAsync();

            return GraphResult.FromGroupedSaleItems(groupedSales);
        }
    }
}
