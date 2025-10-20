using AutoMapper;
using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Models;
using Moq;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;
using MyfirstLib.Services;

namespace GroceryStoreTests.ServiceTests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public sealed class StoreServiceTests
    {
        private Mock<IStoreRepository> _storeRepositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IStoreService _storeService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _mapperMock = new Mock<IMapper>();
            _storeService = new StoreService(_storeRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<Store>
            {
                new Store { StoreId = 1, Name = "Store1" },
                new Store { StoreId = 2, Name = "Store2" }
            };
            var models = new List<StoreModel>
            {
                new StoreModel { StoreId = 1, Name = "Store1" },
                new StoreModel { StoreId = 2, Name = "Store2" }
            };

            _storeRepositoryMock.Setup(x => x.GetAll()).Returns(data);
            _mapperMock.Setup(x => x.Map<IEnumerable<StoreModel>>(data)).Returns(models);

            var stores = _storeService.GetAll();
            Assert.AreEqual(2, stores.Count());
            Assert.IsTrue(stores.Any(x => x.StoreId == 1));
            Assert.IsTrue(stores.Any(x => x.StoreId == 2));
        }

        [TestMethod]
        public void Get_ById_ReturnsStore()
        {
            var store = new Store { StoreId = 5, Name = "TestStore" };
            var model = new StoreModel { StoreId = 5, Name = "TestStore" };

            _storeRepositoryMock.Setup(x => x.GetById(5)).Returns(store);
            _mapperMock.Setup(x => x.Map<StoreModel>(store)).Returns(model);

            var result = _storeService.GetById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result!.StoreId);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var store = new Store { StoreId = 3 };
            var model = new StoreModel { StoreId = 3 };

            _storeRepositoryMock.Setup(x => x.AddAsync(store)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Store>(model)).Returns(store);

            await _storeService.AddAsync(model);

            _storeRepositoryMock.Verify(x => x.AddAsync(store), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var store = new Store { StoreId = 7 };
            var model = new StoreModel { StoreId = 7 };

            _storeRepositoryMock.Setup(x => x.UpdateAsync(store)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Store>(model)).Returns(store);

            await _storeService.UpdateAsync(model);

            _storeRepositoryMock.Verify(x => x.UpdateAsync(store), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var store = new Store { StoreId = 4 };
            var model = new StoreModel { StoreId = 4 };

            _storeRepositoryMock.Setup(x => x.DeleteAsync(store)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Store>(model)).Returns(store);

            await _storeService.DeleteAsync(model);

            _storeRepositoryMock.Verify(x => x.DeleteAsync(store), Times.Once);
        }
    }
}