using Assignment.Apis.Extensions;
using Assignment.Apis.Models;
using Assignment.Dal.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Assignment.Apis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly ILogger<StoresController> _logger;
        private readonly IStoresRepository _storeRepository;

        public StoresController(IStoresRepository storeRepository, ILogger<StoresController> logger)
        {
            _storeRepository = storeRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("{storeName?}")]
        public async Task<IActionResult> GetStoresAsync([FromRoute] string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                _logger.LogInformation("No store name specified. Fetching details of all stores.");
                var stores = await _storeRepository.GetAllStoresAsync();
                return Ok(stores);
            }

            _logger.LogInformation($"Fetching details of store with name '{storeName}'.");
            var store = await _storeRepository.GetStoreAsync(storeName);
            return Ok(store);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStoreAsync([FromBody] StoreInputModel store)
        {
            if (string.IsNullOrWhiteSpace(store.StoreName) || string.IsNullOrWhiteSpace(store.Country)
                || string.IsNullOrWhiteSpace(store.City))
            {
                return BadRequest("Invalid input.");
            }

            var storeDetails = await _storeRepository.GetStoreAsync(store.StoreName);
            if (storeDetails != null)
            {
                var errorMessage = $"Store with name '{store.StoreName}' already registered. Register with a different store name.";
                _logger.LogInformation(errorMessage);
                return BadRequest(errorMessage);
            }

            var storeDal = store.ToDalEntity();
            await _storeRepository.RegisterStoreAsync(storeDal);
            _logger.LogInformation($"Store with name '{store.StoreName}' registered successfully.");
            return Ok();
            // return Created();
        }

        [HttpPut]
        [Route("{storeName}")]
        public async Task<IActionResult> UpdateStoreAsync([FromRoute] string storeName, [FromBody] StoreInputModel store)
        {
            if (string.IsNullOrWhiteSpace(store.StoreName) || string.IsNullOrWhiteSpace(store.Country)
                || string.IsNullOrWhiteSpace(store.City))
            {
                return BadRequest("Invalid input.");
            }

            if (storeName.ToLower() != store.StoreName.ToLower())
            {
                // TODO: check what to return
                return BadRequest();
            }

            var storeDetails = await _storeRepository.GetStoreAsync(store.StoreName);
            if (storeDetails == null)
            {
                var errorMessage = $"Store with name '{storeName}' doesn't exist.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var storeDal = store.ToDalEntity();
            storeDal.Id = storeDetails.Id;
            await _storeRepository.UpdateStoreAsync(storeDal);
            _logger.LogInformation($"Details of store with name '{store.StoreName}' updated successfully.");
            return NoContent();
        }

        [HttpDelete]
        [Route("{storeName}")]
        public async Task<IActionResult> RemoveStoreAsync([FromRoute] string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                return BadRequest("Invalid store name provided.");
            }

            var isRegistered = await _storeRepository.StoreExistsAsync(storeName);
            if (isRegistered)
            {
                await _storeRepository.RemoveStoreAsync(storeName);
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
    }
}
