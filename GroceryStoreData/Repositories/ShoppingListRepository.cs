using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreData.Repositories
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly GroceryStore_DataContext _context;

        public ShoppingListRepository(GroceryStore_DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ShoppingList> GetAll()
        {
            return _context.ShoppingLists
                .Include(x => x.User)
                .Include(x => x.Store)
                    .ThenInclude(x => x.State)
                .ToList();
        }

        public ShoppingList? GetById(int id)
        {
            return _context.ShoppingLists
                .Include(x => x.User)
                .Include(x => x.Store)
                    .ThenInclude(x => x.State)
                .FirstOrDefault(x => x.ShoppingListId == id);
        }

        public async Task AddAsync(ShoppingList shoppingList)
        {
            if (shoppingList is null)
            {
                throw new ArgumentNullException(nameof(shoppingList));
            }

            _context.ShoppingLists.Add(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShoppingList shoppingList)
        {
            if (shoppingList is null)
            {
                throw new ArgumentNullException(nameof(shoppingList));
            }

            _context.ShoppingLists.Update(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ShoppingList shoppingList)
        {
            if (shoppingList is null)
            {
                return;
            }

            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();
        }
    }
}