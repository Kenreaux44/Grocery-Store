using GroceryStoreData.Models;
using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IUserService
    {
        UserModel? GetById(int id);
        List<UserModel> GetAll();
        Task AddAsync(UserModel user);
        Task UpdateAsync(UserModel user);
        Task DeleteAsync(UserModel user);
    }
}