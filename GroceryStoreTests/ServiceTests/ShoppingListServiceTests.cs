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
    public class ShoppingListServiceTests
    {
        private Mock<IShoppingListRepository> _repositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IShoppingListService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IShoppingListRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ShoppingListService(_repositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var lists = new List<ShoppingList>
            {
                new ShoppingList { ShoppingListId = 1, Title = "Groceries" },
                new ShoppingList { ShoppingListId = 2, Title = "Party" }
            };
            var models = new List<ShoppingListModel>
            {
                new ShoppingListModel { ShoppingListId = 1, Title = "Groceries" },
                new ShoppingListModel { ShoppingListId = 2, Title = "Party" }
            };

            _repositoryMock.Setup(r => r.GetAll()).Returns(lists);
            _mapperMock.Setup(m => m.Map<IEnumerable<ShoppingListModel>>(lists)).Returns(models);

            var result = _service.GetAll();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Groceries", result.First().Title);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var list = new ShoppingList { ShoppingListId = 1, Title = "Groceries" };
            var model = new ShoppingListModel { ShoppingListId = 1, Title = "Groceries" };

            _repositoryMock.Setup(r => r.GetById(1)).Returns(list);
            _mapperMock.Setup(m => m.Map<ShoppingListModel>(list)).Returns(model);

            var result = _service.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Groceries", result!.Title);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var list = new ShoppingList { ShoppingListId = 1, Title = "Groceries" };
            var model = new ShoppingListModel { ShoppingListId = 1, Title = "Groceries" };

            _repositoryMock.Setup(r => r.AddAsync(list)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ShoppingList>(model)).Returns(list);

            await _service.AddAsync(model);

            _repositoryMock.Verify(r => r.AddAsync(list), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var list = new ShoppingList { ShoppingListId = 1, Title = "Groceries" };
            var model = new ShoppingListModel { ShoppingListId = 1, Title = "Groceries" };

            _repositoryMock.Setup(r => r.UpdateAsync(list)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ShoppingList>(model)).Returns(list);

            await _service.UpdateAsync(model);

            _repositoryMock.Verify(r => r.UpdateAsync(list), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var list = new ShoppingList { ShoppingListId = 1, Title = "Groceries" };
            var model = new ShoppingListModel { ShoppingListId = 1, Title = "Groceries" };

            _repositoryMock.Setup(r => r.DeleteAsync(list)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ShoppingList>(model)).Returns(list);

            await _service.DeleteAsync(model);

            _repositoryMock.Verify(r => r.DeleteAsync(list), Times.Once);
        }
    }
}