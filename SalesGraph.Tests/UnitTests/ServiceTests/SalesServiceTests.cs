using Microsoft.EntityFrameworkCore;
using SalesGraph.Core.DataAccess.Context;
using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.Core.Services.Interfaces;
using SalesGraph.Tests.Infrastructure.Interfaces;
using static SalesGraph.Core.Infrastructure.Enums.Enums;

namespace SalesGraph.Tests.UnitTests.ServiceTests
{
    public class SaleTestFixture : IIsolated
    {
        private readonly SalesGraphContext _context;
        
        public SaleTestFixture(SalesGraphContext context)
        {
            _context = context;
        }

        public Guid InitTestProduct()
        {
            Guid id = Guid.NewGuid();
            _context.Products.Add(new Product()
            {
                Name = "Test Product",
                Cost = 100,
                ProductId = id
            });
            _context.SaveChanges(); 
            ProductId = id;
            
            return id;
        }

        public async Task Isolate()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [SalesGraphTest].[dbo].[Sales]");
        }

        public Guid ProductId { get; private set; }
    }

    [Collection("SalesGraphTests")]
    public class SalesServiceTests : IClassFixture<SaleTestFixture>
    {
        private readonly ISalesService _salesService;
        private readonly SaleTestFixture _fixture;

        public SalesServiceTests(ISalesService salesService, SaleTestFixture fixture)
        {
            _salesService = salesService;
            _fixture = fixture;
            _fixture.InitTestProduct();
        }

        [Fact]
        public async Task CreateSaleItem_ValidItem_ReturnsSaleItem()
        {
            await _fixture.Isolate();

            // Arrange
            var saleItem = new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = _fixture.ProductId, 
                SaleDate = DateTime.Now,
                Quantity = 5,
            };
           
            // Act
            var result = await _salesService.CreateSaleItem(saleItem);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(saleItem.SaleItemId, result.SaleItemId);
        }

        [Fact]
        public async Task CreateSaleItem_InvalidItem_ThrowsException()
        {
            await _fixture.Isolate();

            // Arrange
            var saleItem = new SaleItem(); // Assuming this is an invalid SaleItem

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _salesService.CreateSaleItem(saleItem));
        }

        [Fact]
        public async Task UpdateSaleItem_ValidItem_ReturnsUpdatedSaleItem()
        {
            await _fixture.Isolate();

            // Arrange
            var saleItem = new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = _fixture.ProductId, // Assuming this is a valid ProductId
                SaleDate = DateTime.Now,
                Quantity = 5,
            };

            // Create the sale item
            await _salesService.CreateSaleItem(saleItem);

            // Modify some properties
            saleItem.Quantity = 10;

            // Act
            var updatedSaleItem = await _salesService.UpdateSaleItem(saleItem);

            // Assert
            Assert.NotNull(updatedSaleItem);
            Assert.Equal(10, updatedSaleItem.Quantity);
        }

        [Fact]
        public async Task UpdateSaleItem_InvalidItem_ThrowsException()
        {
            await _fixture.Isolate();

            // Arrange
            var saleItem = new SaleItem(); // Assuming this is an invalid SaleItem

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _salesService.UpdateSaleItem(saleItem));
        }

        [Fact]
        public async Task DeleteSaleItem_ExistingItem_ReturnsTrue()
        {
            await _fixture.Isolate();

            // Arrange
            var saleItem = new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = _fixture.ProductId,
                SaleDate = DateTime.Now,
                Quantity = 5,
            };

            // Create the sale item
            await _salesService.CreateSaleItem(saleItem);

            // Act
            var result = await _salesService.DeleteSaleItem(saleItem.SaleItemId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteSaleItem_NonExistingItem_ReturnsFalse()
        {
            await _fixture.Isolate();

            // Arrange
            var nonExistingSaleItemId = Guid.NewGuid(); // Assuming this ID doesn't exist in the database

            // Act
            var result = await _salesService.DeleteSaleItem(nonExistingSaleItemId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetSaleItemsPagesCount_ValidProductId_ReturnsPageCount()
        {
            await _fixture.Isolate();

            // Arrange
            var productId = _fixture.ProductId;
            var saleItems = Enumerable.Range(1, 20).Select(i => new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = productId,
                SaleDate = DateTime.Now.AddDays(-i), // Assuming these are valid sale dates
                Quantity = i,
            });

            foreach (var saleItem in saleItems)
            {
                await _salesService.CreateSaleItem(saleItem);
            }

            // Set up pagination
            var pageSize = 5;

            // Act
            var pageCount = await _salesService.GetSaleItemsPagesCount(productId, pageSize);

            // Assert
            Assert.Equal(4, pageCount); // Assuming there are 20 sale items and the page size is 5
        }

        [Fact]
        public async Task GetSaleItemsByProductId_ValidProductId_ReturnsSaleItems()
        {
            await _fixture.Isolate();

            // Arrange
            var productId = _fixture.ProductId;
            var saleItems = Enumerable.Range(1, 10).Select(i => new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = productId,
                SaleDate = DateTime.Now.AddDays(-i),
                Quantity = i,
            });

            foreach (var saleItem in saleItems)
            {
                await _salesService.CreateSaleItem(saleItem);
            }

            // Set up pagination
            var pageSize = 5;
            var pageNumber = 2;
            var totalPages = 2; 

            // Act
            var retrievedSaleItems = await _salesService.GetSaleItemsByProductId(productId, pageSize, pageNumber, totalPages);

            // Assert
            Assert.NotNull(retrievedSaleItems);
            Assert.Equal(5, retrievedSaleItems.Count); // Assuming the page size is 5
        }

        [Fact]
        public async Task GetSalesGroupedByTimePeriod_ValidPeriod_ReturnsGraphResult()
        {
            await _fixture.Isolate();

            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2024, 1, 1);
            var groupingParam = GroupingParam.Month; 
            var saleItems = Enumerable.Range(1, 10).Select(i => new SaleItem
            {
                SaleItemId = Guid.NewGuid(),
                ProductId = _fixture.ProductId,
                SaleDate = endDate.AddDays(-i),
                Quantity = i,
            });
            foreach (var saleItem in saleItems)
            {
                await _salesService.CreateSaleItem(saleItem);
            }

            // Act
            var result = await _salesService.GetSalesGroupedByTimePeriod(startDate, endDate, groupingParam);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.X);
            Assert.NotEmpty(result.Y);
            Assert.NotEmpty(result.Z);
        }
    }
}
