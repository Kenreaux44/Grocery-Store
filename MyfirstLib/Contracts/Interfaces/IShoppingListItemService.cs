using GroceryStoreData.Models;
using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IShoppingListItemService
    {
        IEnumerable<ShoppingListItemModel> GetAll();
        ShoppingListItemModel? GetById(int id);
        Task AddAsync(ShoppingListItemModel shoppingListItem);
        Task UpdateAsync(ShoppingListItemModel shoppingListItem);
        Task DeleteAsync(ShoppingListItemModel shoppingListItem);
    }
}