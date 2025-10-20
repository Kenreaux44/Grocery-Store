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
    public sealed class StoreRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IStoreRepository _storeRepository;

        [TestInitialize]
        public void Initialize()
        {
            _storeRepository = new StoreRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<Store>
            {
                new Store { StoreId = 1, Name = "Store1", Address1 = "Addr1", City = "City1", StateId = 1, PostalCode = "11111", CreatedDate = System.DateTime.Now, CreatedBy = "Test" },
                new Store { StoreId = 2, Name = "Store2", Address1 = "Addr2", City = "City2", StateId = 2, PostalCode = "22222", CreatedDate = System.DateTime.Now, CreatedBy = "Test" }
            }.AsQueryable();

            var mockStoresDbSet = new Mock<DbSet<Store>>();
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Stores).Returns(mockStoresDbSet.Object);

            var stores = _storeRepository.GetAll();
            Assert.AreEqual(2, stores.Count());
            Assert.AreEqual("Store1", stores.First(x => x.StoreId == 1).Name);
            Assert.AreEqual("Store2", stores.First(x => x.StoreId == 2).Name);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var data = new List<Store>
            {
                new Store { StoreId = 1, Name = "Store1", Address1 = "Addr1", City = "City1", StateId = 1, PostalCode = "11111", CreatedDate = System.DateTime.Now, CreatedBy = "Test" },
                new Store { StoreId = 2, Name = "Store2", Address1 = "Addr2", City = "City2", StateId = 2, PostalCode = "22222", CreatedDate = System.DateTime.Now, CreatedBy = "Test" }
            }.AsQueryable();

            var mockStoresDbSet = new Mock<DbSet<Store>>();
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Stores).Returns(mockStoresDbSet.Object);

            var store = _storeRepository.GetById(1);
            Assert.IsNotNull(store);
            Assert.AreEqual("Store1", store.Name);
        }

        [TestMethod]
        public async Task AddAsync_AddsStore()
        {
            var store = new Store { StoreId = 3, Name = "Store3", Address1 = "Addr3", City = "City3", StateId = 3, PostalCode = "33333", CreatedDate = System.DateTime.Now, CreatedBy = "Test" };
            var mockStoresDbSet = new Mock<DbSet<Store>>();
            _dbContext.Setup(x => x.Stores).Returns(mockStoresDbSet.Object);

            await _storeRepository.AddAsync(store);

            mockStoresDbSet.Verify(x => x.Add(store), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesStore()
        {
            var store = new Store { StoreId = 1 };
            var mockStoresDbSet = new Mock<DbSet<Store>>();

            _dbContext.Setup(x => x.Stores).Returns(mockStoresDbSet.Object);

            store.Name = "Store1A";

            await _storeRepository.UpdateAsync(store);

            mockStoresDbSet.Verify(x => x.Update(store), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_RemovesStore()
        {
            var store = new Store { StoreId = 4, Name = "Store4", Address1 = "Addr4", City = "City4", StateId = 4, PostalCode = "44444", CreatedDate = System.DateTime.Now, CreatedBy = "Test" };
            var data = new List<Store> { store }.AsQueryable();

            var mockStoresDbSet = new Mock<DbSet<Store>>();
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStoresDbSet.As<IQueryable<Store>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Stores).Returns(mockStoresDbSet.Object);

            var storeFromRepo = _storeRepository.GetById(4);

            Assert.IsNotNull(storeFromRepo);
            Assert.AreEqual(4, storeFromRepo.StoreId);

            await _storeRepository.DeleteAsync(storeFromRepo);

            mockStoresDbSet.Verify(x => x.Remove(store), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}