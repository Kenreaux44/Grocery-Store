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
    public sealed class ProductRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IProductRepository _productRepository;

        [TestInitialize]
        public void Initialize()
        {
            _productRepository = new ProductRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<Product>
            {
                new Product { ProductId = 1, Name = "Apple", UnitOfMeasure = "Each" },
                new Product { ProductId = 2, Name = "Banana", UnitOfMeasure = "Each" }
            }.AsQueryable();

            var mockProductsDbSet = new Mock<DbSet<Product>>();
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Products).Returns(mockProductsDbSet.Object);

            var products = _productRepository.GetAll();
            Assert.AreEqual(2, products.Count());
            Assert.AreEqual("Apple", products.First(x => x.ProductId == 1).Name);
            Assert.AreEqual("Banana", products.First(x => x.ProductId == 2).Name);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var data = new List<Product>
            {
                new Product { ProductId = 1, Name = "Apple", UnitOfMeasure = "Each" }
            }.AsQueryable();

            var mockProductsDbSet = new Mock<DbSet<Product>>();
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockProductsDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Products).Returns(mockProductsDbSet.Object);

            var product = _productRepository.GetById(1);
            Assert.IsNotNull(product);
            Assert.AreEqual(1, product.ProductId);
            Assert.AreEqual("Apple", product.Name);
        }

        [TestMethod]
        public async Task AddAsync_AddsProduct()
        {
            var product = new Product { ProductId = 3, Name = "Orange", UnitOfMeasure = "Each" };
            var mockProductsDbSet = new Mock<DbSet<Product>>();
            _dbContext.Setup(x => x.Products).Returns(mockProductsDbSet.Object);

            await _productRepository.AddAsync(product);

            mockProductsDbSet.Verify(x => x.Add(product), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesProduct()
        {
            var product = new Product { ProductId = 4, Name = "Grape", UnitOfMeasure = "Each" };
            var mockProductsDbSet = new Mock<DbSet<Product>>();
            _dbContext.Setup(x => x.Products).Returns(mockProductsDbSet.Object);

            product.Name = "Apple";

            await _productRepository.UpdateAsync(product);

            mockProductsDbSet.Verify(x => x.Update(product), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesProduct()
        {
            var product = new Product { ProductId = 5, Name = "Pear", UnitOfMeasure = "Each" };
            var mockProductsDbSet = new Mock<DbSet<Product>>();
            _dbContext.Setup(x => x.Products).Returns(mockProductsDbSet.Object);

            await _productRepository.DeleteAsync(product);

            mockProductsDbSet.Verify(x => x.Remove(product), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}