using GroceryStoreData.Models;

namespace GroceryStoreData.Contracts.Interfaces
{
    public interface IStoreRepository
    {
        IEnumerable<Store> GetAll();
        Store? GetById(int id);
        Task AddAsync(Store store);
        Task UpdateAsync(Store store);
        Task DeleteAsync(Store store);
    }
}
