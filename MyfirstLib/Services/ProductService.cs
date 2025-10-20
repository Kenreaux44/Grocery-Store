using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper
        )
        {
            _repository = productRepository;
            _mapper = mapper;
        }

        public IEnumerable<ProductModel> GetAll()
        {
            var products = _repository.GetAll();

            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public ProductModel? GetById(int id)
        {
            var product = _repository.GetById(id);
            return _mapper.Map<ProductModel?>(product);
        }

        public async Task AddAsync(ProductModel product)
        {
            var entity = _mapper.Map<Product>(product);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ProductModel product)
        {
            var entity = _mapper.Map<Product>(product);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(ProductModel product)
        {
            var entity = _mapper.Map<Product>(product);
            await _repository.DeleteAsync(entity);
        }
    }
}