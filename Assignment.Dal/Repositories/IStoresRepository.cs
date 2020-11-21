using Assignment.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.Dal.Repositories
{
    public interface IStoresRepository
    {
        Task<Store> GetStoreAsync(string storeName);
        Task RegisterStoreAsync(Store store);
        Task RemoveStoreAsync(string storeName);
        Task<bool> StoreExistsAsync(string storeName);
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task UpdateStoreAsync(Store store);
    }
}