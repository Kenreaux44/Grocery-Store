using GroceryStoreData.Models;

namespace GroceryStoreData.Contracts.Interfaces
{
    public interface IStoreProductRepository
    {
        IEnumerable<StoreProduct> GetAll();
        StoreProduct? GetById(int id);
        Task AddAsync(StoreProduct storeProduct);
        Task UpdateAsync(StoreProduct storeProduct);
        Task DeleteAsync(StoreProduct storeProduct);
    }
}