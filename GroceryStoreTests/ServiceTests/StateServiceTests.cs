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
    public sealed class StateServiceTests
    {
        private Mock<IStateRepository> _stateRepositoryMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private IStateService _stateService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _stateRepositoryMock = new Mock<IStateRepository>();
            _mapperMock = new Mock<IMapper>();
            _stateService = new StateService(_stateRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Get_All_ReturnsExpectedData()
        {
            var data = new List<State>
            {
                new State { StateId = 1 },
                new State { StateId = 2 }
            };
            var models = new List<StateModel>
            {
                new StateModel { StateId = 1 },
                new StateModel { StateId = 2 }
            };

            _stateRepositoryMock.Setup(x => x.GetAll()).Returns(data);
            _mapperMock.Setup(x => x.Map<IEnumerable<StateModel>>(data)).Returns(models);

            var states = _stateService.GetAll();
            Assert.AreEqual(2, states.Count());
            Assert.IsTrue(states.Any(x => x.StateId == 1));
            Assert.IsTrue(states.Any(x => x.StateId == 2));
        }

        [TestMethod]
        public void Get_ById_ReturnsState()
        {
            var state = new State { StateId = 5 };
            var model = new StateModel { StateId = 5 };

            _stateRepositoryMock.Setup(x => x.GetById(5)).Returns(state);
            _mapperMock.Setup(x => x.Map<StateModel>(state)).Returns(model);

            var result = _stateService.GetById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.StateId);
        }

        [TestMethod]
        public void GetByAbbreviation_ReturnsState()
        {
            var state = new State { StateId = 6, Abbreviation = "CA" };
            var model = new StateModel { StateId = 6, Abbreviation = "CA" };

            _stateRepositoryMock.Setup(x => x.GetByAbbreviation("CA")).Returns(state);
            _mapperMock.Setup(x => x.Map<StateModel>(state)).Returns(model);

            var result = _stateService.GetByAbbreviation("CA");
            Assert.IsNotNull(result);
            Assert.AreEqual("CA", result.Abbreviation);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepository()
        {
            var state = new State { StateId = 3 };
            var model = new StateModel { StateId = 3 };

            _stateRepositoryMock.Setup(x => x.AddAsync(state)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<State>(model)).Returns(state);

            await _stateService.AddAsync(model);

            _stateRepositoryMock.Verify(x => x.AddAsync(state), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepository()
        {
            var state = new State { StateId = 7, Abbreviation = "TS" };
            var model = new StateModel { StateId = 7, Abbreviation = "TS" };

            _stateRepositoryMock.Setup(x => x.UpdateAsync(state)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<State>(model)).Returns(state);

            await _stateService.UpdateAsync(model);

            _stateRepositoryMock.Verify(x => x.UpdateAsync(state), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepository()
        {
            var state = new State { StateId = 4 };
            var model = new StateModel { StateId = 4 };

            _stateRepositoryMock.Setup(x => x.DeleteAsync(state)).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<State>(model)).Returns(state);

            await _stateService.DeleteAsync(model);

            _stateRepositoryMock.Verify(x => x.DeleteAsync(state), Times.Once);
        }
    }
}