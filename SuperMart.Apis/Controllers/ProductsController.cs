using SuperMart.Apis.Extensions;
using SuperMart.Dal.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMart.Apis.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository, ILogger<ProductsController> logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] string searchText = null)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _logger.LogInformation("No search text specified. Fetching all products.");
            }
            (var productsDal, var elapsedTime) = await _productsRepository.GetAllProductsAsync(searchText);
            var products = productsDal.Select(product => product.ToOutputModel());

            return Ok(new
            {
                ExecutionTime = $"{elapsedTime} ms.",
                RecordsFetched = products.Count(),
                Result = products
            });
        }
    }
}
