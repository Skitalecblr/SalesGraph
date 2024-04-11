using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesGraph.Core.Services.Interfaces;
using SalesGraph.ViewModels.Results;
using System;
using System.Threading.Tasks;

namespace SalesGraph.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _productService.GetAllProducts();
                return Ok(new ProductList().ToResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetails(Guid id)
        {
            try
            {
                var result = await _productService.GetProductById(id);
                if (result == null)
                    return NotFound();

                return Ok(new ProductDetail().ToResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product details.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }
    }
}