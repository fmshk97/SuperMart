using SuperMart.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMart.Dal.Repositories
{
    public interface IProductsRepository
    {
        Task AddProductAsync(string storeName, Product product);
        Task<(IEnumerable<Product>, long)> GetAllStoreProductsAsync(string storeName);
        Task<Product> GetProductAsync(string storeName, string productName);
        Task RemoveProductAsync(string storeName, string productName);
        Task<(IEnumerable<Product>, long)> GetAllProductsAsync(string searchText = null);
        Task<bool> ProductExistsAsync(string storeName, string productName);
    }
}