using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _repository;
        private readonly IMapper _mapper;

        public StateService(
            IStateRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<StateModel> GetAll()
        {
            var states = _repository.GetAll();

            return _mapper.Map<IEnumerable<StateModel>>(states);
        }

        public StateModel? GetById(int id)
        {
            var state = _repository.GetById(id);

            return _mapper.Map<StateModel>(state);
        }

        public StateModel? GetByAbbreviation(string abbreviation)
        {
            var state = _repository.GetByAbbreviation(abbreviation);

            return _mapper.Map<StateModel>(state);
        }

        public Task AddAsync(StateModel state)
        {
            var entity = _mapper.Map<State>(state);
            return _repository.AddAsync(entity);
        }

        public Task UpdateAsync(StateModel state)
        {
            var entity = _mapper.Map<State>(state);
            return _repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(StateModel state)
        {
            var entity = _mapper.Map<State>(state);
            return _repository.DeleteAsync(entity);
        }
    }
}