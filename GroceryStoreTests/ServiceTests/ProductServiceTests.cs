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
    public sealed class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IProductService _productService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<Product>
            {
                new Product { ProductId = 1, Name = "Apple", UnitOfMeasure = "Each" },
                new Product { ProductId = 2, Name = "Banana", UnitOfMeasure = "Each" }
            };
            var models = new List<ProductModel>
            {
                new ProductModel { ProductId = 1, Name = "Apple", UnitOfMeasure = "Each" },
                new ProductModel { ProductId = 2, Name = "Banana", UnitOfMeasure = "Each" }
            };

            _productRepositoryMock.Setup(x => x.GetAll()).Returns(data);
            _mapperMock.Setup(x => x.Map<IEnumerable<ProductModel>>(data)).Returns(models);

            var products = _productService.GetAll();
            Assert.AreEqual(2, products.Count());
            Assert.IsTrue(products.Any(x => x.ProductId == 1));
            Assert.IsTrue(products.Any(x => x.ProductId == 2));
        }

        [TestMethod]
        public void Get_ById_ReturnsProduct()
        {
            var product = new Product { ProductId = 5, Name = "TestProduct" };
            var model = new ProductModel { ProductId = 5, Name = "TestProduct" };

            _productRepositoryMock.Setup(x => x.GetById(5)).Returns(product);
            _mapperMock.Setup(x => x.Map<ProductModel>(product)).Returns(model);

            var result = _productService.GetById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result!.ProductId);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var product = new Product { ProductId = 3 };
            var model = new ProductModel { ProductId = 3 };

            _productRepositoryMock.Setup(x => x.AddAsync(product)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductModel>())).Returns(product);

            await _productService.AddAsync(model);

            _productRepositoryMock.Verify(x => x.AddAsync(product), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var product = new Product { ProductId = 7 };
            var model = new ProductModel { ProductId = 3 };

            _productRepositoryMock.Setup(x => x.UpdateAsync(product)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductModel>())).Returns(product);

            await _productService.UpdateAsync(model);

            _productRepositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var product = new Product { ProductId = 4 };
            var model = new ProductModel { ProductId = 4 };

            _productRepositoryMock.Setup(x => x.DeleteAsync(product)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductModel>())).Returns(product);

            await _productService.DeleteAsync(model);

            _productRepositoryMock.Verify(x => x.DeleteAsync(product), Times.Once);
        }
    }
}