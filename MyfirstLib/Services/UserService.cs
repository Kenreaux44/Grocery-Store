using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstLib.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository repository,
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public UserModel? GetById(int id)
        {
            var user = _repository.GetById(id);

            return _mapper.Map<UserModel>(user);
        }

        public List<UserModel> GetAll()
        {
            var users = _repository.GetAll();

            return _mapper.Map<List<UserModel>>(users);
        }

        public async Task AddAsync(UserModel user)
        {
            var entity = _mapper.Map<User>(user);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(UserModel user)
        {
            var entity = _mapper.Map<User>(user);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(UserModel user)
        {
            var entity = _mapper.Map<User>(user);
            await _repository.DeleteAsync(entity);
        }
    }
}