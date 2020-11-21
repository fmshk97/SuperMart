using Assignment.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Dal.Repositories
{
    public class StoresRepository : Repository, IStoresRepository
    {
        private readonly SuperMartDbContext _context;
        private readonly ILogger<StoresRepository> _logger;

        public StoresRepository(SuperMartDbContext context, ILogger<StoresRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RegisterStoreAsync(Store store)
        {
            if (store == null)
            {
                _logger.LogInformation($"Failed to register the store. Invalid store details.");
                return;
            }

            try
            {
                await _context.AddAsync(store);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Store with name '{store.StoreName}' registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register store with name '{store.StoreName}'. Details: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveStoreAsync(string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                return;
            }

            try
            {
                var store = await _context.Stores.FirstOrDefaultAsync(x => x.StoreName == storeName);
                if (store == null)
                {
                    _logger.LogWarning($"Store with name '{storeName}' doesn't exist.");
                }
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Store with name '{storeName}' removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove store with name '{storeName}'. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> StoreExistsAsync(string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                return false;
            }

            try
            {
                var exists = await _context.Stores.AnyAsync(x =>
                                x.StoreName.ToLower() == storeName.ToLower());
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch store with name '{storeName}'. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<Store> GetStoreAsync(string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                return null;
            }

            try
            {
                return await _context.Stores.Include(store => store.Products)
                                            .Where(store => store.StoreName.ToLower() == storeName.ToLower())
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch store with name '{storeName}'. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            try
            {
                return await _context.Stores.Include(store => store.Products)
                                            .AsNoTracking()
                                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch all the stores. Details: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateStoreAsync(Store store)
        {
            if (store == null)
            {
                return;
            }

            try
            {
                var storeDetails = await _context.Stores.FirstOrDefaultAsync(x => x.Id == store.Id);
                if (storeDetails != null)
                {
                    storeDetails.City = store.City;
                    storeDetails.Country = store.Country;
                    storeDetails.Pin = store.Pin;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogError($"Store '{store.StoreName}' doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update the store '{store.StoreName}' details. Details: {ex.Message}");
                throw;
            }
        }
    }
}
