using Microsoft.EntityFrameworkCore;
using SalesGraph.Core.DataAccess.Context;
using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.Core.Services.Implementation;
using SalesGraph.Core.Services.Interfaces;
using SalesGraph.Tests.Infrastructure.Interfaces;

namespace SalesGraph.Tests.UnitTests.ServiceTests
{
    public class ProductTestsFixture : IIsolated
    {
        private readonly SalesGraphContext _context;

        public ProductTestsFixture(SalesGraphContext context)
        {
            _context = context;
        }

        public async Task Isolate()
        {
            await _context.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE [SalesGraphTest].[dbo].[Sales]"); 
            await _context.Database.ExecuteSqlRawAsync(@"DELETE FROM [SalesGraphTest].[dbo].[Products]");
        }

        public async Task CreateSalesForProduct(Guid productId, int count = 10)
        {
            var saleItems = Enumerable.Range(1, count).Select(i => new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = productId,
                SaleDate = DateTime.Now.AddDays(-i),
                Quantity = 1,
            });
            await _context.Sales.AddRangeAsync(saleItems.ToList());
            await _context.SaveChangesAsync();

        }
    }

    [Collection("SalesGraphTests")]
    public class ProductServiceTests : IClassFixture<ProductTestsFixture>
    {
        private readonly IProductService _productService;
        private readonly ProductTestsFixture _fixture;

        public ProductServiceTests(IProductService productService, ProductTestsFixture fixture)
        {
            _productService = productService;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetProductById_ExistingProduct_ReturnsProductWithTotalSales()
        {
            await _fixture.Isolate();

            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Cost = 100
            };
            await _productService.CreateProduct(product);
            await _fixture.CreateSalesForProduct(product.ProductId, 3);

            // Act
            var result = await _productService.GetProductById(product.ProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.ProductId, result.ProductId);
            Assert.Equal(3, result.TotalSales);
        }

        [Fact]
        public async Task GetProductById_NonExistingProduct_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = Guid.NewGuid(), Name = "Product 1", Cost = 100},
                new Product { ProductId = Guid.NewGuid(), Name = "Product 2" , Cost = 100},
                new Product { ProductId = Guid.NewGuid(), Name = "Product 3" , Cost = 100}
            };

            await _productService.CreateProduct(products[0]);
            await _productService.CreateProduct(products[1]);
            await _productService.CreateProduct(products[2]);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.Equal(products.Count, result.Count);
            Assert.All(result, p => Assert.Contains(products, prod => prod.ProductId == p.ProductId));
        }
    }
}
