using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreData.Repositories
{
    public class StoreProductRepository : IStoreProductRepository
    {
        private readonly GroceryStore_DataContext _context;

        public StoreProductRepository(GroceryStore_DataContext context)
        {
            _context = context;
        }

        public IEnumerable<StoreProduct> GetAll()
        {
            return _context.StoreProducts
                .Include(x => x.Store)
                .Include(x => x.Product)
                .ToList();
        }

        public StoreProduct? GetById(int id)
        {
            return _context.StoreProducts
                .Include(x => x.Store)
                .Include(x => x.Product)
                .FirstOrDefault(x => x.StoreProductId == id);
        }

        public async Task AddAsync(StoreProduct storeProduct)
        {
            if (storeProduct is null)
            {
                throw new ArgumentNullException(nameof(storeProduct));
            }

            _context.StoreProducts.Add(storeProduct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StoreProduct storeProduct)
        {
            if (storeProduct is null)
            {
                throw new ArgumentNullException(nameof(storeProduct));
            }

            _context.StoreProducts.Update(storeProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(StoreProduct storeProduct)
        {
            if (storeProduct is null)
            {
                return;
            }

            _context.StoreProducts.Remove(storeProduct);
            await _context.SaveChangesAsync();
        }
    }
}