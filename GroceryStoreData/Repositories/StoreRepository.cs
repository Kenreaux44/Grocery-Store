using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreData.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly GroceryStore_DataContext _context;

        public StoreRepository(GroceryStore_DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Store> GetAll()
        {
            return _context.Stores
                .Include(x => x.State)
                .ToList();
        }

        public Store? GetById(int id)
        {
            return _context.Stores
                .Include(x => x.State)
                .FirstOrDefault(x => x.StoreId == id);
        }

        public async Task AddAsync(Store store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Store store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            _context.Stores.Update(store);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Store store)
        {
            if (store is null)
            {
                return;
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }
    }
}