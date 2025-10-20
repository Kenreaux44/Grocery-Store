using GroceryStoreData.Models;
using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IStoreProductService
    {
        IEnumerable<StoreProductModel> GetAll();
        StoreProductModel? GetById(int id);
        Task AddAsync(StoreProductModel storeProduct);
        Task UpdateAsync(StoreProductModel storeProduct);
        Task DeleteAsync(StoreProductModel storeProduct);
    }
}