using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesGraph.Core.DataAccess.Context;
using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.Core.Services.Interfaces;
namespace SalesGraph.Core.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly SalesGraphContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(SalesGraphContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                product.ProductId = Guid.NewGuid();
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while adding the product to the database.");
                throw new Exception("An error occurred while adding the product to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the product.");
                throw new Exception("An unexpected error occurred while adding the product.");
            }
        }

        public async Task<Product> GetProductById(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product != null)
                {
                    var totalSales = await _context.Sales
                        .Where(s => s.ProductId == id)
                        .CountAsync();

                    product.TotalSales = totalSales;
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product.");
                throw new Exception("An error occurred while retrieving the product.", ex);
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the products.");
                throw new Exception("An error occurred while retrieving the products.", ex);
            }
        }
    }
}
