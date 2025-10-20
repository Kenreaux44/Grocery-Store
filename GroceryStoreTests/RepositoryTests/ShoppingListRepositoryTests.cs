using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Models;
using GroceryStoreData.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GroceryStoreTests.RepositoryTests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public sealed class ShoppingListRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IShoppingListRepository _shoppingListRepository;

        [TestInitialize]
        public void Initialize()
        {
            _shoppingListRepository = new ShoppingListRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<ShoppingList>
            {
                new ShoppingList { ShoppingListId = 1, Title = "Groceries", UserId = 1, StoreId = 1, CreatedDate = DateTime.Now, CreatedBy = "Test" },
                new ShoppingList { ShoppingListId = 2, Title = "Party", UserId = 2, StoreId = 2, CreatedDate = DateTime.Now, CreatedBy = "Test" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ShoppingList>>();
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.ShoppingLists).Returns(mockDbSet.Object);

            var shoppingLists = _shoppingListRepository.GetAll();
            Assert.AreEqual(2, shoppingLists.Count());
            Assert.AreEqual("Groceries", shoppingLists.First(x => x.ShoppingListId == 1).Title);
            Assert.AreEqual("Party", shoppingLists.First(x => x.ShoppingListId == 2).Title);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var data = new List<ShoppingList>
            {
                new ShoppingList { ShoppingListId = 1, Title = "Groceries", UserId = 1, StoreId = 1, CreatedDate = DateTime.Now, CreatedBy = "Test" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ShoppingList>>();
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<ShoppingList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.ShoppingLists).Returns(mockDbSet.Object);

            var shoppingList = _shoppingListRepository.GetById(1);
            Assert.IsNotNull(shoppingList);
            Assert.AreEqual(1, shoppingList.ShoppingListId);
            Assert.AreEqual("Groceries", shoppingList.Title);
        }

        [TestMethod]
        public async Task AddAsync_AddsShoppingList()
        {
            var shoppingList = new ShoppingList { ShoppingListId = 3, Title = "Weekly", UserId = 1, StoreId = 1, CreatedDate = DateTime.Now, CreatedBy = "Test" };
            var mockDbSet = new Mock<DbSet<ShoppingList>>();
            _dbContext.Setup(x => x.ShoppingLists).Returns(mockDbSet.Object);

            await _shoppingListRepository.AddAsync(shoppingList);

            mockDbSet.Verify(x => x.Add(shoppingList), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesShoppingList()
        {
            var shoppingList = new ShoppingList { ShoppingListId = 4, Title = "Monthly", UserId = 2, StoreId = 2, CreatedDate = DateTime.Now, CreatedBy = "Test" };
            var mockDbSet = new Mock<DbSet<ShoppingList>>();
            _dbContext.Setup(x => x.ShoppingLists).Returns(mockDbSet.Object);

            await _shoppingListRepository.UpdateAsync(shoppingList);

            mockDbSet.Verify(x => x.Update(shoppingList), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesShoppingList()
        {
            var shoppingList = new ShoppingList { ShoppingListId = 5, Title = "Special", UserId = 3, StoreId = 3, CreatedDate = DateTime.Now, CreatedBy = "Test" };
            var mockDbSet = new Mock<DbSet<ShoppingList>>();
            _dbContext.Setup(x => x.ShoppingLists).Returns(mockDbSet.Object);

            await _shoppingListRepository.DeleteAsync(shoppingList);

            mockDbSet.Verify(x => x.Remove(shoppingList), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}