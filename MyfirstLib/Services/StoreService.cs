using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repository;
        private readonly IMapper _mapper;

        public StoreService(
            IStoreRepository storeRepository,
            IMapper mapper
        )
        {
            _repository = storeRepository;
            _mapper = mapper;
        }

        public IEnumerable<StoreModel> GetAll()
        {
            var store = _repository.GetAll();

            return _mapper.Map<IEnumerable<StoreModel>>(store);
        }

        public StoreModel? GetById(int id)
        {
            var store = _repository.GetById(id);

            return _mapper.Map<StoreModel>(store);
        }

        public async Task AddAsync(StoreModel store)
        {
            var entity = _mapper.Map<Store>(store);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(StoreModel store)
        {
            var entity = _mapper.Map<Store>(store);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(StoreModel store)
        {
            var entity = _mapper.Map<Store>(store);
            await _repository.DeleteAsync(entity);
        }
    }
}