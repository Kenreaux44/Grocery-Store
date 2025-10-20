using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreData.Repositories
{
    public class ShoppingListItemRepository : IShoppingListItemRepository
    {
        private readonly GroceryStore_DataContext _context;

        public ShoppingListItemRepository(GroceryStore_DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ShoppingListItem> GetAll()
        {
            return _context.ShoppingListItems
                .Include(x => x.ShoppingList)
                .Include(x => x.StoreProduct)
                    .ThenInclude(sp => sp.Product)
                .ToList();
        }

        public ShoppingListItem? GetById(int id)
        {
            return _context.ShoppingListItems
                .Include(x => x.ShoppingList)
                .Include(x => x.StoreProduct)
                    .ThenInclude(sp => sp.Product)
                .FirstOrDefault(x => x.ShoppingListItemId == id);
        }

        public async Task AddAsync(ShoppingListItem shoppingListItem)
        {
            if (shoppingListItem is null)
            {
                throw new ArgumentNullException(nameof(shoppingListItem));
            }

            _context.ShoppingListItems.Add(shoppingListItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShoppingListItem shoppingListItem)
        {
            if (shoppingListItem is null)
            {
                throw new ArgumentNullException(nameof(shoppingListItem));
            }

            _context.ShoppingListItems.Update(shoppingListItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ShoppingListItem shoppingListItem)
        {
            if (shoppingListItem is null)
            {
                return;
            }

            _context.ShoppingListItems.Remove(shoppingListItem);
            await _context.SaveChangesAsync();
        }
    }
}