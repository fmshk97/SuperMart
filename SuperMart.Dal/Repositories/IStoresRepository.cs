using SuperMart.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMart.Dal.Repositories
{
    public interface IStoresRepository
    {
        Task<Store> GetStoreAsync(string storeName);
        Task RegisterStoreAsync(Store store);
        Task RemoveStoreAsync(string storeName);
        Task<bool> StoreExistsAsync(string storeName);
        Task<(IEnumerable<Store>, long)> GetAllStoresAsync();
        Task UpdateStoreAsync(Store store);
    }
}