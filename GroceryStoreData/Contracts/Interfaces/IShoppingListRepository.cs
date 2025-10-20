using GroceryStoreData.Models;

namespace GroceryStoreData.Contracts.Interfaces
{
    public interface IShoppingListRepository
    {
        IEnumerable<ShoppingList> GetAll();
        ShoppingList? GetById(int id);
        Task AddAsync(ShoppingList shoppingList);
        Task UpdateAsync(ShoppingList shoppingList);
        Task DeleteAsync(ShoppingList shoppingList);
    }
}