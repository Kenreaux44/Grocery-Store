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
    public sealed class ShoppingListItemRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IShoppingListItemRepository _shoppingListItemRepository;

        [TestInitialize]
        public void Initialize()
        {
            _shoppingListItemRepository = new ShoppingListItemRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<ShoppingListItem>
            {
                new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 },
                new ShoppingListItem { ShoppingListItemId = 2, ShoppingListId = 2, StoreProductId = 2, Quantity = 3 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ShoppingListItem>>();
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.ShoppingListItems).Returns(mockDbSet.Object);

            var shoppingListItems = _shoppingListItemRepository.GetAll();
            Assert.AreEqual(2, shoppingListItems.Count());
            Assert.AreEqual(1, shoppingListItems.First(x => x.ShoppingListItemId == 1).ShoppingListId);
            Assert.AreEqual(2, shoppingListItems.First(x => x.ShoppingListItemId == 2).ShoppingListId);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var data = new List<ShoppingListItem>
            {
                new ShoppingListItem { ShoppingListItemId = 1, ShoppingListId = 1, StoreProductId = 1, Quantity = 2 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ShoppingListItem>>();
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<ShoppingListItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.ShoppingListItems).Returns(mockDbSet.Object);

            var item = _shoppingListItemRepository.GetById(1);
            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ShoppingListItemId);
        }

        [TestMethod]
        public async Task AddAsync_AddsItem()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 3, ShoppingListId = 3, StoreProductId = 3, Quantity = 5 };
            var mockDbSet = new Mock<DbSet<ShoppingListItem>>();
            _dbContext.Setup(x => x.ShoppingListItems).Returns(mockDbSet.Object);

            await _shoppingListItemRepository.AddAsync(item);

            mockDbSet.Verify(x => x.Add(item), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesItem()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 4, ShoppingListId = 4, StoreProductId = 4, Quantity = 10 };
            var mockDbSet = new Mock<DbSet<ShoppingListItem>>();
            _dbContext.Setup(x => x.ShoppingListItems).Returns(mockDbSet.Object);

            await _shoppingListItemRepository.UpdateAsync(item);

            mockDbSet.Verify(x => x.Update(item), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesItem()
        {
            var item = new ShoppingListItem { ShoppingListItemId = 5, ShoppingListId = 5, StoreProductId = 5, Quantity = 7 };
            var mockDbSet = new Mock<DbSet<ShoppingListItem>>();
            _dbContext.Setup(x => x.ShoppingListItems).Returns(mockDbSet.Object);

            await _shoppingListItemRepository.DeleteAsync(item);

            mockDbSet.Verify(x => x.Remove(item), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}