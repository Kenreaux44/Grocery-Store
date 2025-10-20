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
    public sealed class UserRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IUserRepository _userRepository;

        [TestInitialize]
        public void Initialize()
        {
            _userRepository = new UserRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<User>
            {
                new User { UserId = 1, Email = "user1@email.com", FirstName = "John", LastName = "Doe", CreatedDate = DateTime.Now, CreatedBy = "Test" },
                new User { UserId = 2, Email = "user2@email.com", FirstName = "Jane", LastName = "Smith", CreatedDate = DateTime.Now, CreatedBy = "Test" }
            }.AsQueryable();

            var mockUsersDbSet = new Mock<DbSet<User>>();
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Users).Returns(mockUsersDbSet.Object);

            var users = _userRepository.GetAll();
            Assert.AreEqual(2, users.Count());
            Assert.AreEqual("John", users.First(x => x.UserId == 1).FirstName);
            Assert.AreEqual("Jane", users.First(x => x.UserId == 2).FirstName);
        }

        [TestMethod]
        public void Get_ById_ReturnsExpectedData()
        {
            var data = new List<User>
            {
                new User { UserId = 1, Email = "user1@email.com", FirstName = "John", LastName = "Doe", CreatedDate = DateTime.Now, CreatedBy = "Test" },
                new User { UserId = 2, Email = "user2@email.com", FirstName = "Jane", LastName = "Smith", CreatedDate = DateTime.Now, CreatedBy = "Test" }
            }.AsQueryable();

            var mockUsersDbSet = new Mock<DbSet<User>>();
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Users).Returns(mockUsersDbSet.Object);

            var user = _userRepository.GetById(1);
            Assert.IsNotNull(user);
            Assert.AreEqual("John", user.FirstName);
        }

        [TestMethod]
        public async Task AddAsync_AddsUser()
        {
            var user = new User { UserId = 3, Email = "user3@email.com", FirstName = "Alice", LastName = "Brown", CreatedDate = DateTime.Now, CreatedBy = "Test" };
            var mockUsersDbSet = new Mock<DbSet<User>>();
            _dbContext.Setup(x => x.Users).Returns(mockUsersDbSet.Object);

            await _userRepository.AddAsync(user);

            mockUsersDbSet.Verify(x => x.Add(user), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesUser()
        {
            var user = new User { UserId = 1 };
            var mockUsersDbSet = new Mock<DbSet<User>>();

            _dbContext.Setup(x => x.Users).Returns(mockUsersDbSet.Object);

            user.FirstName = "JohnUpdated";

            await _userRepository.UpdateAsync(user);

            mockUsersDbSet.Verify(x => x.Update(user), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_RemovesUser()
        {
            var user = new User { UserId = 4, Email = "user4@email.com", FirstName = "Bob", LastName = "White", CreatedDate = DateTime.Now, CreatedBy = "Test" };
            var data = new List<User> { user }.AsQueryable();

            var mockUsersDbSet = new Mock<DbSet<User>>();
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockUsersDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.Users).Returns(mockUsersDbSet.Object);

            var userFromRepo = _userRepository.GetById(4);

            Assert.IsNotNull(userFromRepo);
            Assert.AreEqual(4, userFromRepo.UserId);

            await _userRepository.DeleteAsync(userFromRepo);

            mockUsersDbSet.Verify(x => x.Remove(user), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}