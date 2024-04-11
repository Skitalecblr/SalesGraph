using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesGraph.Core.Infrastructure.Enums;
using SalesGraph.Core.Services.Interfaces;
using SalesGraph.ViewModels.Parameters;
using SalesGraph.ViewModels.Results;
using System;
using System.Threading.Tasks;

namespace SalesGraph.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISalesService _salesService;

        public SaleController(ILogger<SaleController> logger, ISalesService salesService)
        {
            _logger = logger;
            _salesService = salesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid productId, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                var pagesCount = await _salesService.GetSaleItemsPagesCount(productId, pageSize);
                var sales = await _salesService.GetSaleItemsByProductId(productId, pageSize, pageNumber, pagesCount);
                return Ok(new SaleList().ToResult(sales, pagesCount));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sales.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(EditSaleItem model)
        {
            try
            {
                await _salesService.UpdateSaleItem(model.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sale.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSaleItem model)
        {
            try
            {
                await _salesService.CreateSaleItem(model.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding sale.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _salesService.DeleteSaleItem(id);
                if (result)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sale.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetSalesStats(int period, string startDate, string endDate)
        {
            try
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);

                var result = await _salesService.GetSalesGroupedByTimePeriod(start, end, (Enums.GroupingParam)period);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sales statistics.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
            }
        }
    }
}
