using Microsoft.EntityFrameworkCore;
using SalesGraph.Core.DataAccess.Models.Entities;

namespace SalesGraph.Core.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductById(Guid id);

        Task<List<Product>> GetAllProducts();

        Task<Product> CreateProduct(Product product);
    }
}
