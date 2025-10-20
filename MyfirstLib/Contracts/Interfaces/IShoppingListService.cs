using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IShoppingListService
    {
        IEnumerable<ShoppingListModel> GetAll();
        ShoppingListModel? GetById(int id);
        Task AddAsync(ShoppingListModel shoppingList);
        Task UpdateAsync(ShoppingListModel shoppingList);
        Task DeleteAsync(ShoppingListModel shoppingList);
    }
}