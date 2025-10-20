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
    public sealed class StoreProductServiceTests
    {
        private Mock<IStoreProductRepository> _storeProductRepositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IStoreProductService _storeProductService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _storeProductRepositoryMock = new Mock<IStoreProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _storeProductService = new StoreProductService(_storeProductRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<StoreProduct>
            {
                new StoreProduct { StoreProductId = 1, StoreId = 1, ProductId = 1 },
                new StoreProduct { StoreProductId = 2, StoreId = 2, ProductId = 2 }
            };
            var models = new List<StoreProductModel>
            {
                new StoreProductModel { StoreProductId = 1, StoreId = 1, ProductId = 1 },
                new StoreProductModel { StoreProductId = 2, StoreId = 2, ProductId = 2 }
            };

            _storeProductRepositoryMock.Setup(x => x.GetAll()).Returns(data);
            _mapperMock.Setup(m => m.Map<IEnumerable<StoreProductModel>>(It.IsAny<IEnumerable<StoreProduct>>())).Returns(models);

            var storeProducts = _storeProductService.GetAll();
            Assert.AreEqual(2, storeProducts.Count());
            Assert.IsTrue(storeProducts.Any(x => x.StoreProductId == 1));
            Assert.IsTrue(storeProducts.Any(x => x.StoreProductId == 2));
        }

        [TestMethod]
        public void Get_ById_ReturnsStoreProduct()
        {
            var storeProduct = new StoreProduct { StoreProductId = 5, StoreId = 1, ProductId = 1 };
            var model = new StoreProductModel { StoreProductId = 5, StoreId = 1, ProductId = 1 };

            _storeProductRepositoryMock.Setup(x => x.GetById(5)).Returns(storeProduct);
            _mapperMock.Setup(m => m.Map<StoreProductModel>(It.IsAny<StoreProduct>())).Returns(model);

            var result = _storeProductService.GetById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result!.StoreProductId);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var storeProduct = new StoreProduct { StoreProductId = 3 };
            var model = new StoreProductModel { StoreProductId = 3 };

            _storeProductRepositoryMock.Setup(x => x.AddAsync(storeProduct)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<StoreProduct>(It.IsAny<StoreProductModel>())).Returns(storeProduct);

            await _storeProductService.AddAsync(model);

            _storeProductRepositoryMock.Verify(x => x.AddAsync(storeProduct), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var storeProduct = new StoreProduct { StoreProductId = 7 };
            var model = new StoreProductModel { StoreProductId = 7 };

            _storeProductRepositoryMock.Setup(x => x.UpdateAsync(storeProduct)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<StoreProduct>(It.IsAny<StoreProductModel>())).Returns(storeProduct);

            await _storeProductService.UpdateAsync(model);

            _storeProductRepositoryMock.Verify(x => x.UpdateAsync(storeProduct), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var storeProduct = new StoreProduct { StoreProductId = 4 };
            var model = new StoreProductModel { StoreProductId = 4 };

            _storeProductRepositoryMock.Setup(x => x.DeleteAsync(storeProduct)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<StoreProduct>(It.IsAny<StoreProductModel>())).Returns(storeProduct);

            await _storeProductService.DeleteAsync(model);

            _storeProductRepositoryMock.Verify(x => x.DeleteAsync(storeProduct), Times.Once);
        }
    }
}