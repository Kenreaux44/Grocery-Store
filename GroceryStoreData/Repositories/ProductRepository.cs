using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;

namespace GroceryStoreData.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GroceryStore_DataContext _context;

        public ProductRepository(GroceryStore_DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.ProductId == id);
        }

        public async Task AddAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            if (product is null)
            {
                return;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}