using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class StoreProductService : IStoreProductService
    {
        private readonly IStoreProductRepository _repository;
        private readonly IMapper _mapper;

        public StoreProductService(
            IStoreProductRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<StoreProductModel> GetAll()
        {
            var storeProducts = _repository.GetAll();

            return _mapper.Map<IEnumerable<StoreProductModel>>(storeProducts);
        }

        public StoreProductModel? GetById(int id)
        {
            var storeProduct = _repository.GetById(id);

            return _mapper.Map<StoreProductModel>(storeProduct);
        }

        public async Task AddAsync(StoreProductModel storeProduct)
        {
            var entity = _mapper.Map<StoreProduct>(storeProduct);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(StoreProductModel storeProduct)
        {
            var entity = _mapper.Map<StoreProduct>(storeProduct);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(StoreProductModel storeProduct)
        {
            var entity = _mapper.Map<StoreProduct>(storeProduct);
            await _repository.DeleteAsync(entity);
        }
    }
}