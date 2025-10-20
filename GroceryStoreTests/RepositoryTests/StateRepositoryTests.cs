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
    public sealed class StateRepositoryTests
    {
        private Mock<GroceryStore_DataContext> _dbContext = new Mock<GroceryStore_DataContext>();
        private IStateRepository _stateRepository;

        [TestInitialize]
        public void Initialize()
        {
            _stateRepository = new StateRepository(_dbContext.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<State>
            {
                new State { StateId = 1 },
                new State { StateId = 2 }
            }.AsQueryable();

            var mockStatesDbSet = new Mock<DbSet<State>>();
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.States).Returns(mockStatesDbSet.Object);

            var states = _stateRepository.GetAll();
            Assert.AreEqual(2, states.Count());
            Assert.IsTrue(states.Any(x => x.StateId == 1));
            Assert.IsTrue(states.Any(x => x.StateId == 2));
        }

        [TestMethod]
        public void Get_ById_ReturnsState()
        {
            var state = new State { StateId = 5 };
            var data = new List<State> { state }.AsQueryable();

            var mockStatesDbSet = new Mock<DbSet<State>>();
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.States).Returns(mockStatesDbSet.Object);

            var result = _stateRepository.GetById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.StateId);
        }

        [TestMethod]
        public void Get_ByAbbreviation_ReturnsState()
        {
            var state = new State { StateId = 6 };
            typeof(State).GetProperty("Abbreviation")?.SetValue(state, "CA");
            var data = new List<State> { state }.AsQueryable();

            var mockStatesDbSet = new Mock<DbSet<State>>();
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.States).Returns(mockStatesDbSet.Object);

            var result = _stateRepository.GetByAbbreviation("CA");
            Assert.IsNotNull(result);
            Assert.AreEqual("CA", typeof(State).GetProperty("Abbreviation")?.GetValue(result));
        }

        [TestMethod]
        public async Task AddAsync_AddsState()
        {
            var state = new State { StateId = 3 };
            var mockStatesDbSet = new Mock<DbSet<State>>();
            _dbContext.Setup(x => x.States).Returns(mockStatesDbSet.Object);

            await _stateRepository.AddAsync(state);

            mockStatesDbSet.Verify(x => x.Add(state), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesState()
        {
            var state = new State { StateId = 7 };
            var mockStatesDbSet = new Mock<DbSet<State>>();

            _dbContext.Setup(x => x.States).Returns(mockStatesDbSet.Object);

            state.Abbreviation = "TS";

            await _stateRepository.UpdateAsync(state);

            mockStatesDbSet.Verify(x => x.Update(state), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_RemovesState()
        {
            var state = new State { StateId = 4 };
            var data = new List<State> { state }.AsQueryable();

            var mockStatesDbSet = new Mock<DbSet<State>>();
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(data.Provider);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(data.Expression);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockStatesDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContext.Setup(x => x.States).Returns(mockStatesDbSet.Object);

            var stateFromRepo = _stateRepository.GetById(4);

            Assert.IsNotNull(stateFromRepo);
            Assert.AreEqual(4, stateFromRepo.StateId);

            await _stateRepository.DeleteAsync(stateFromRepo);

            mockStatesDbSet.Verify(x => x.Remove(It.Is<State>(s => s.StateId == 4)), Times.Once);
            _dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}