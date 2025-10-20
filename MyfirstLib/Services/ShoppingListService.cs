using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListRepository _repository;
        private readonly IMapper _mapper;

        public ShoppingListService(
            IShoppingListRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<ShoppingListModel> GetAll()
        {
            var shoppingLists = _repository.GetAll();

            return _mapper.Map<IEnumerable<ShoppingListModel>>(shoppingLists);
        }

        public ShoppingListModel? GetById(int id)
        {
            var shoppingList = _repository.GetById(id);

            return _mapper.Map<ShoppingListModel>(shoppingList);
        }

        public async Task AddAsync(ShoppingListModel shoppingList)
        {
            var entity = _mapper.Map<ShoppingList>(shoppingList);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ShoppingListModel shoppingList)
        {
            var entity = _mapper.Map<ShoppingList>(shoppingList);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(ShoppingListModel shoppingList)
        {
            var entity = _mapper.Map<ShoppingList>(shoppingList);
            await _repository.DeleteAsync(entity);
        }
    }
}