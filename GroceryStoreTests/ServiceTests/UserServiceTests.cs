using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using Moq;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;
using MyfirstLib.Services;
using System.Runtime.InteropServices.Marshalling;

namespace GroceryStoreTests.ServiceTests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public sealed class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IUserService _userService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<User>
            {
                new User { UserId = 1, FirstName = "John" },
                new User { UserId = 2, FirstName = "Jane" }
            };

            var models = new List<UserModel>
            {
                new UserModel { UserId = 1, FirstName = "John" },
                new UserModel { UserId = 2, FirstName = "Jane" }
            };

            _userRepositoryMock.Setup(x => x.GetAll()).Returns(data); _userRepositoryMock.Setup(x => x.GetAll()).Returns(data);
            _mapperMock.Setup(x => x.Map<IEnumerable<UserModel>>(data)).Returns(models);

            var users = _userService.GetAll();
            Assert.AreEqual(2, users.Count());
            Assert.IsTrue(users.Any(x => x.UserId == 1));
            Assert.IsTrue(users.Any(x => x.UserId == 2));
        }

        [TestMethod]
        public void Get_ById_ReturnsUser()
        {
            var user = new User { UserId = 5, FirstName = "TestUser" };
            var model = new UserModel { UserId = 5, FirstName = "TestUser" };

            _userRepositoryMock.Setup(x => x.GetById(5)).Returns(user);
            _mapperMock.Setup(x => x.Map<UserModel>(user)).Returns(model);

            var result = _userService.GetById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result!.UserId);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var user = new User { UserId = 3 };
            var model = new UserModel { UserId = 3 };

            _userRepositoryMock.Setup(x => x.AddAsync(user)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<User>(model)).Returns(user);

            await _userService.AddAsync(model);

            _userRepositoryMock.Verify(x => x.AddAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var user = new User { UserId = 7 };
            var model = new UserModel { UserId = 7 };

            _userRepositoryMock.Setup(x => x.UpdateAsync(user)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<User>(model)).Returns(user);

            await _userService.UpdateAsync(model);

            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var user = new User { UserId = 4 };
            var model = new UserModel { UserId = 4 };

            _userRepositoryMock.Setup(x => x.DeleteAsync(user)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<User>(model)).Returns(user);

            await _userService.DeleteAsync(model);

            _userRepositoryMock.Verify(x => x.DeleteAsync(user), Times.Once);
        }
    }
}