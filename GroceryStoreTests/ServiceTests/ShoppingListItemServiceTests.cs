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
    public class ShoppingListItemServiceTests
    {
        private Mock<IShoppingListItemRepository> _repositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IShoppingListItemService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IShoppingListItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ShoppingListItemService(_repositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var items = new List<ShoppingListItem>
            {
                new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 },
                new ShoppingListItem { ShoppingListItemId = 2, ShoppingListId = 2, StoreProductId = 2, Quantity = 3 }
            };
            var models = new List<ShoppingListItemModel>
            {
                new ShoppingListItemModel { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 },
                new ShoppingListItemModel { ShoppingListItemId = 2, ShoppingListId = 2, StoreProductId = 2, Quantity = 3 }
            };

            _repositoryMock.Setup(r => r.GetAll()).Returns(items);
            _mapperMock.Setup(m => m.Map<IEnumerable<ShoppingListItemModel>>(items)).Returns(models);

            var result = _service.GetAll();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.First().ShoppingListId);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };
            var model = new ShoppingListItemModel { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };

            _repositoryMock.Setup(r => r.GetById(1)).Returns(item);
            _mapperMock.Setup(m => m.Map<ShoppingListItemModel>(item)).Returns(model);

            var result = _service.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result!.ShoppingListId);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };
            var model = new ShoppingListItemModel { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };

            _repositoryMock.Setup(r => r.AddAsync(item)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ShoppingListItem>(model)).Returns(item);

            await _service.AddAsync(model);

            _repositoryMock.Verify(r => r.AddAsync(item), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };
            var model = new ShoppingListItemModel { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };

            _repositoryMock.Setup(r => r.UpdateAsync(item)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ShoppingListItem>(model)).Returns(item);

            await _service.UpdateAsync(model);

            _repositoryMock.Verify(r => r.UpdateAsync(item), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };
            var model = new ShoppingListItemModel { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 };

            _repositoryMock.Setup(r => r.DeleteAsync(item)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ShoppingListItem>(model)).Returns(item);

            await _service.DeleteAsync(model);

            _repositoryMock.Verify(r => r.DeleteAsync(item), Times.Once);
        }
    }
}