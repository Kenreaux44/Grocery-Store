using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using GroceryStoreData.Repositories;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class ShoppingListItemService : IShoppingListItemService
    {
        private readonly IShoppingListItemRepository _repository;
        private readonly IMapper _mapper;

        public ShoppingListItemService(
            IShoppingListItemRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<ShoppingListItemModel> GetAll()
        {
            var shoppingListItems = _repository.GetAll();

            return _mapper.Map<IEnumerable<ShoppingListItemModel>>(shoppingListItems);
        }

        public ShoppingListItemModel? GetById(int id)
        {
            var shoppingListItem = _repository.GetById(id);
            
            return _mapper.Map<ShoppingListItemModel?>(shoppingListItem);
        }

        public async Task AddAsync(ShoppingListItemModel shoppingListItem)
        {
            var entity = _mapper.Map<ShoppingListItem>(shoppingListItem);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ShoppingListItemModel shoppingListItem)
        {
            var entity = _mapper.Map<ShoppingListItem>(shoppingListItem);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(ShoppingListItemModel shoppingListItem)
        {
            var entity = _mapper.Map<ShoppingListItem>(shoppingListItem);
            await _repository.DeleteAsync(entity);
        }
    }
}