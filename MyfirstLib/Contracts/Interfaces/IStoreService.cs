using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IStoreService
    {
        IEnumerable<StoreModel> GetAll();
        StoreModel? GetById(int id);
        Task AddAsync(StoreModel store);
        Task UpdateAsync(StoreModel store);
        Task DeleteAsync(StoreModel store);
    }
}