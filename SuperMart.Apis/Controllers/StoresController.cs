using SuperMart.Apis.Extensions;
using SuperMart.Apis.Models;
using SuperMart.Dal.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Routing;

namespace SuperMart.Apis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly ILogger<StoresController> _logger;
        private readonly IStoresRepository _storesRepository;
        private readonly IProductsRepository _productsRepository;

        public StoresController(IStoresRepository storesRepository, 
            IProductsRepository productsRepository, 
            ILogger<StoresController> logger)
        {
            _storesRepository = storesRepository;
            _productsRepository = productsRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetStoresAsync()
        {
            (var storesDal, var elapsedTime) = await _storesRepository.GetAllStoresAsync();
            var stores = storesDal.Select(store => store.ToOutputModel());

            _logger.LogInformation($"Successfully fetched details of all the stores in {elapsedTime} ms.");
            
            return Ok(new
            {
                ExecutionTime = $"{elapsedTime} ms.",
                RecordsFetched = stores.Count(),
                Result = stores
            });
        }

        [HttpGet]
        [Route("{storeName}")]
        public async Task<IActionResult> GetStoreAsync([FromRoute] string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                var errorMessage = "Invalid store name.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var store = (await _storesRepository.GetStoreAsync(storeName)).ToOutputModel();
            _logger.LogInformation($"Successfully fetched details of the store with name '{storeName}'.");
            return Ok(store);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStoreAsync([FromBody] StoreInputModel storeInput)
        {
            if (string.IsNullOrWhiteSpace(storeInput.StoreName) || string.IsNullOrWhiteSpace(storeInput.Country)
                || string.IsNullOrWhiteSpace(storeInput.City))
            {
                var errorMessage = "Invalid input.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var storeDetails = await _storesRepository.GetStoreAsync(storeInput.StoreName);
            if (storeDetails != null)
            {
                var errorMessage = $"Store with name '{storeInput.StoreName}' already registered. Register with a different store name.";
                _logger.LogInformation(errorMessage);
                return BadRequest(errorMessage);
            }

            var storeDal = storeInput.ToDalEntity();
            await _storesRepository.RegisterStoreAsync(storeDal);
            _logger.LogInformation($"Store with name '{storeInput.StoreName}' registered successfully.");
            return Ok($"Store '{storeInput.StoreName}' registered Successfully.");
        }

        [HttpPut]
        [Route("{storeName}")]
        public async Task<IActionResult> UpdateStoreAsync([FromRoute] string storeName, [FromBody] StoreInputModel store)
        {
            if (string.IsNullOrWhiteSpace(store.StoreName) || string.IsNullOrWhiteSpace(store.Country)
                || string.IsNullOrWhiteSpace(store.City))
            {
                var errorMessage = "Invalid input.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            if (storeName.ToLower() != store.StoreName.ToLower())
            {
                return RedirectPermanent("");
            }

            var storeDetails = await _storesRepository.GetStoreAsync(store.StoreName);
            if (storeDetails == null)
            {
                var errorMessage = $"Store with name '{storeName}' doesn't exist.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var storeDal = store.ToDalEntity();
            storeDal.Id = storeDetails.Id;
            await _storesRepository.UpdateStoreAsync(storeDal);
            _logger.LogInformation($"Details of store with name '{store.StoreName}' updated successfully.");
            return NoContent();
        }

        [HttpDelete]
        [Route("{storeName}")]
        public async Task<IActionResult> RemoveStoreAsync([FromRoute] string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                var errorMessage = "Invalid store name provided.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var isRegistered = await _storesRepository.StoreExistsAsync(storeName);
            if (isRegistered)
            {
                await _storesRepository.RemoveStoreAsync(storeName);
                _logger.LogInformation($"Successfully removed store with name '{storeName}'.");
                return NoContent();
            }
            else
            {
                var message = $"Store with name '{storeName}' not found.";
                _logger.LogWarning(message);
                return NotFound(message);
            }
        }

        [HttpGet]
        [Route("{storeName}/products")]
        public async Task<IActionResult> GetStoreProductsAsync([FromRoute] string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                var errorMessage = "Invalid store name provided.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            (var productDal, var elapsedTime) = await _productsRepository.GetAllStoreProductsAsync(storeName);
            var products = productDal.Select(product => product.ToOutputModel());

            return Ok(new 
            { 
                ExecutionTime = $"{elapsedTime} ms.", 
                RecordsFetched = products.Count(), 
                Result = products 
            });
        }

        [HttpPost]
        [Route("{storeName}/products")]
        public async Task<IActionResult> AddProductToStoreAsync([FromRoute] string storeName, [FromBody] ProductInputModel productInput)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                var errorMessage = "Invalid store name provided.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            if (productInput == null || string.IsNullOrWhiteSpace(productInput.Name) || string.IsNullOrWhiteSpace(productInput.Category))
            {
                var errorMessage = "Invalid product details provided.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var isRegistered = await _storesRepository.StoreExistsAsync(storeName);
            if (!isRegistered)
            {
                var errorMessage = $"Store '{storeName}' is not registered.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var product = await _productsRepository.GetProductAsync(storeName, productInput.Name);
            if (product != null)
            {
                var errorMessage = $"Product '{productInput.Name}' already exists in the store '{storeName}'.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var productDal = productInput.ToDalEntity();
            await _productsRepository.AddProductAsync(storeName, productDal);
            _logger.LogInformation($"Product '{productInput.Name}' successfully added to the store '{storeName}'.");

            return Ok($"Product '{productInput.Name}' added Successfully to the store '{storeName}'.");
        }

        [HttpDelete]
        [Route("{storeName}/products/{productName}")]
        public async Task<IActionResult> RemoveProductAsync([FromRoute] string storeName, [FromRoute] string productName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                var errorMessage = "Invalid store name provided.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                var errorMessage = "Invalid product name provided.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var product = await _productsRepository.GetProductAsync(storeName, productName);
            if (product == null)
            {
                var errorMessage = $"Product '{productName}' not found in the store '{storeName}'.";
                _logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            await _productsRepository.RemoveProductAsync(storeName, productName);
            _logger.LogInformation($"Successfully removed product '{productName}' from store '{storeName}'.");
            return NoContent();
        }
    }
}
