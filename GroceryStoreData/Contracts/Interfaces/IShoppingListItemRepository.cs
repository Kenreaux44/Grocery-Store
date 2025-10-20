using GroceryStoreData.Models;

namespace GroceryStoreData.Contracts.Interfaces
{
    public interface IShoppingListItemRepository
    {
        IEnumerable<ShoppingListItem> GetAll();
        ShoppingListItem? GetById(int id);
        Task AddAsync(ShoppingListItem shoppingListItem);
        Task UpdateAsync(ShoppingListItem shoppingListItem);
        Task DeleteAsync(ShoppingListItem shoppingListItem);
    }
}