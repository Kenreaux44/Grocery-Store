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
    public sealed class StoreProductRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IStoreProductRepository _storeProductRepository;

        [TestInitialize]
        public void Initialize()
        {
            _storeProductRepository = new StoreProductRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<StoreProduct>
            {
                new StoreProduct { StoreProductId = 1, StoreId = 1, ProductId = 1 },
                new StoreProduct { StoreProductId = 2, StoreId = 2, ProductId = 2 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<StoreProduct>>();
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.StoreProducts).Returns(mockDbSet.Object);

            var storeProducts = _storeProductRepository.GetAll();
            Assert.AreEqual(2, storeProducts.Count());
            Assert.AreEqual(1, storeProducts.First(x => x.StoreProductId == 1).StoreId);
            Assert.AreEqual(2, storeProducts.First(x => x.StoreProductId == 2).StoreId);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var data = new List<StoreProduct>
            {
                new StoreProduct { StoreProductId = 1, StoreId = 1, ProductId = 1 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<StoreProduct>>();
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<StoreProduct>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.StoreProducts).Returns(mockDbSet.Object);

            var storeProduct = _storeProductRepository.GetById(1);
            Assert.IsNotNull(storeProduct);
            Assert.AreEqual(1, storeProduct.StoreProductId);
            Assert.AreEqual(1, storeProduct.StoreId);
        }

        [TestMethod]
        public async Task AddAsync_AddsStoreProduct()
        {
            var storeProduct = new StoreProduct { StoreProductId = 3, StoreId = 1, ProductId = 2 };
            var mockDbSet = new Mock<DbSet<StoreProduct>>();
            _dbContext.Setup(x => x.StoreProducts).Returns(mockDbSet.Object);

            await _storeProductRepository.AddAsync(storeProduct);

            mockDbSet.Verify(x => x.Add(storeProduct), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesStoreProduct()
        {
            var storeProduct = new StoreProduct { StoreProductId = 4, StoreId = 2, ProductId = 3 };
            var mockDbSet = new Mock<DbSet<StoreProduct>>();
            _dbContext.Setup(x => x.StoreProducts).Returns(mockDbSet.Object);

            await _storeProductRepository.UpdateAsync(storeProduct);

            mockDbSet.Verify(x => x.Update(storeProduct), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesStoreProduct()
        {
            var storeProduct = new StoreProduct { StoreProductId = 5, StoreId = 3, ProductId = 4 };
            var mockDbSet = new Mock<DbSet<StoreProduct>>();
            _dbContext.Setup(x => x.StoreProducts).Returns(mockDbSet.Object);

            await _storeProductRepository.DeleteAsync(storeProduct);

            mockDbSet.Verify(x => x.Remove(storeProduct), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}