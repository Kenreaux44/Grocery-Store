using MyfirstLib.Models;

namespace MyfirstLib.Contracts.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductModel> GetAll();
        ProductModel? GetById(int id);
        Task AddAsync(ProductModel product);
        Task UpdateAsync(ProductModel product);
        Task DeleteAsync(ProductModel product);
    }
}