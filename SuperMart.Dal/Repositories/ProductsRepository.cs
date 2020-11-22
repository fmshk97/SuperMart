using SuperMart.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMart.Dal.Repositories
{
    public class ProductsRepository : Repository, IProductsRepository
    {
        private readonly SuperMartDbContext _context;
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(SuperMartDbContext context, ILogger<ProductsRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductAsync(string storeName, Product product)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                return;
            }

            if (product == null)
            {
                throw new ArgumentNullException("Product should not be null.");
            }

            try
            {
                var store = await _context.Stores
                    .FirstOrDefaultAsync(store => store.StoreName.ToLower() == storeName.ToLower());
                if (store == null)
                {
                    _logger.LogError($"Failed to add product. Store '{storeName}' is not registered.");
                    return;
                }

                product.Store = store;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product with name '{product.Name}' added to the database successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add product. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ProductExistsAsync(string storeName, string productName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                _logger.LogError("Store name not specified.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                _logger.LogError("Product name not specified.");
                return false;
            }

            try
            {
                var exists = await _context.Products
                    .Include(product => product.Store)
                    .AnyAsync(product =>
                                product.Store.StoreName.ToLower() == storeName.ToLower() &&
                                product.Name.ToLower() == productName.ToLower());
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to search product '{productName}' in store '{storeName}'. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> GetProductAsync(string storeName, string productName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                _logger.LogError("Store name not specified.");
                return null;
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                _logger.LogError("Product name not specified.");
                return null;
            }

            try
            {
                var product = await _context
                    .Products
                    .Include(product => product.Store)
                    .Where(product => product.Store.StoreName.ToLower() == storeName.ToLower() && productName.ToLower() == product.Name)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    _logger.LogError($"Product '{productName}' not found for the store '{storeName}'.");
                    return null;
                }

                _logger.LogInformation($"Product '{productName}' fetched from the database for the store '{storeName}'.");
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch product '{productName}' from the store '{storeName}'. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Product>, long)> GetAllProductsAsync(string searchText = null)
        {
            try
            {
                List<Product> products;
                var stopwatch = new Stopwatch();
                
                stopwatch.Start();
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    products = await _context.Products
                        .Include(product => product.Store)
                        .AsNoTracking()
                        .ToListAsync();
                }
                else
                {
                    products = await _context.Products
                        .Where(product => product.Name.ToLower().Contains(searchText.ToLower()))
                        .Include(product => product.Store)
                        .AsNoTracking()
                        .ToListAsync();
                }
                stopwatch.Stop();

                _logger.LogInformation($"{products.Count} products fetched from the database in {stopwatch.ElapsedMilliseconds} ms.");
                return (products, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch products from the database. Details: {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Product>, long)> GetAllStoreProductsAsync(string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                return (null, 0);
            }

            try
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();
                var products = await _context.Products
                        .Where(product => product.Store.StoreName.ToLower() == storeName)
                        .Include(product => product.Store)
                        .AsNoTracking()
                        .ToListAsync();
                stopwatch.Stop();

                _logger.LogInformation($"{products.Count} products fetched from the database in {stopwatch.ElapsedMilliseconds} ms.");
                return (products, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch products from the database. Details: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveProductAsync(string storeName, string productName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                _logger.LogError("Store name not specified.");
                return;
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                _logger.LogError("Product name not specified.");
                return;
            }

            try
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(x =>
                            x.Name.ToLower() == productName.ToLower() &&
                            x.Store.StoreName.ToLower() == storeName.ToLower());

                if (product == null)
                {
                    _logger.LogError($"Product with name '{productName}' doesn't exist in the store '{storeName}'.");
                    return;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product with name '{productName}' removed successfully from the store '{storeName}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove product '{productName}' from the store '{storeName}'. Details: {ex.Message}");
                throw;
            }
        }
    }
}
