using Avb.Linq.Common.Contracts.Services;
using Avb.Linq.Common.Contracts.Services.Models;
using Avb.Linq.Common.Contracts.Services.Models.AvbTags;
using Avb.Linq.Common.Services;
using Avb.Linq.Tags.Comm.Contracts.Interfaces;
using Avb.Linq.Tags.Comm.Contracts.Models.Responses;
using AVB.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace AVB.UnitTests;

[TestClass]
[TestCategory(TestingConstants.UnitTest)]
public class GatewayControllerServiceTests
{
	private Mock<IGatewayService> _gatewayService;
	private Mock<IGatewayStoreService> _gatewayStoreService;
	private Mock<IStoreCredentialService> _storeCredentialService;
	private Mock<IAvbTagsGatewayCommClient> _avbTagsGatewayCommClient;
	private Mock<ILogger<GatewayControllerService>> _logger;
	
	private IGatewayControllerService _gatewayControllerService;

	[TestInitialize]
    public async Task TestInitialize()
    {
        var gatewayValidator = new GatewayValidator();
		_gatewayService = new Mock<IGatewayService>();
		_gatewayStoreService = new Mock<IGatewayStoreService>();
		_storeCredentialService = new Mock<IStoreCredentialService>();
		_avbTagsGatewayCommClient = new Mock<IAvbTagsGatewayCommClient>();
		_logger = new Mock<ILogger<GatewayControllerService>>();

		_gatewayControllerService = new GatewayControllerService(
			gatewayValidator,
			_gatewayService.Object,
			_gatewayStoreService.Object,
			_storeCredentialService.Object,
			_avbTagsGatewayCommClient.Object,
			_logger.Object
		);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	[DataRow(0, 0, "GatewayId cannot be 0 | StoreId cannot be 0")]
	[DataRow(0, 2, "GatewayId cannot be 0")]
	[DataRow(1, 0, "StoreId cannot be 0")]
	public async Task MoveToNewStore_InvalidIds_ExpectedError(int gatewayId, int newStoreId, string expectedErrorMessage)
    {
		try
		{
			await _gatewayControllerService.MoveToNewStore(gatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual(expectedErrorMessage, ex.Message, true);
			throw;
		}
    }

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_StoreCredentialNotFound_ExpectedError()
	{
		int newStoreId = 3294;
		int currentGatewayId = 132;


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => null);

		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("Store credentials for the provided store were not found in the data store.", ex.Message, true);
			throw;
		}
	}

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_GatewayNotFound_ExpectedError()
	{
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => null);


		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("Requested gateway was not found in the data store.", ex.Message, true);

			_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);

			throw;
		}
	}

	[TestMethod]
	public async Task MoveToNewStore_AlreadyMoved_Success()
	{
		int currentStoreId = 3294;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentGatewayId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "78b20b8aa06287b32add51c6", Name = gatewayName, Mac = gatewayMacAddress });
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_avbTagsGatewayCommClient.Setup(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_gatewayStoreService.Setup(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()))
			.ReturnsAsync(() => new GatewayStoreModel() { GatewayId = currentGatewayId, StoreId = newStoreId });


		var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);

		Assert.AreEqual(true, response.Success);
		Assert.AreEqual("Gateway has already been moved.", response.Message, true);

		_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Exactly(2));
		_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
		_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Exactly(2));
		_avbTagsGatewayCommClient.Verify(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Never);
		_gatewayStoreService.Verify(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()), Times.Never);
	}

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_GatewayInMultipleCloudStoresButRemoveFailed_ExpectedError()
	{
		int currentStoreId = 9;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentStoreId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress, StoreId = currentAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "78b20b8aa06287b32add51c6", Name = gatewayName, Mac = gatewayMacAddress, StoreId = newAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 500 });

		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("Unable to remove the orphaned previous gateway in the Avb Tags Cloud.", ex.Message, true);

			_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);
			_storeCredentialService.Verify(x => x.GetById(currentStoreId), Times.Once);
			_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);

			throw;
		}
	}

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_GatewayInMultipleCloudStoresButUpdateGatewayStoreFailed_ExpectedError()
	{
		int currentStoreId = 9;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentStoreId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress, StoreId = currentAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "78b20b8aa06287b32add51c6", Name = gatewayName, Mac = gatewayMacAddress, StoreId = newAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_gatewayStoreService.Setup(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()))
			.ReturnsAsync(() => null);

		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("Unable to update the gateway store association in the data store.", ex.Message, true);

			_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);
			_storeCredentialService.Verify(x => x.GetById(currentStoreId), Times.Once);
			_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);

			throw;
		}
	}

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_GatewayInMultipleCloudStoresButUpdateGatewayStoreIncorrect_ExpectedError()
	{
		int currentStoreId = 9;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentStoreId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress, StoreId = currentAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "78b20b8aa06287b32add51c6", Name = gatewayName, Mac = gatewayMacAddress, StoreId = newAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_gatewayStoreService.Setup(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()))
			.ReturnsAsync(() => new GatewayStoreModel() { GatewayId = currentGatewayId, StoreId = currentStoreId });

		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("Gateway / Store association update is invalid.", ex.Message, true);

			_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);
			_storeCredentialService.Verify(x => x.GetById(currentStoreId), Times.Once);
			_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);
			_gatewayStoreService.Verify(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()), Times.Once);

			throw;
		}
	}

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_StandardMoveRemoveFailed_ExpectedError()
	{
		int currentStoreId = 9;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentStoreId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => null);
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 400 });


		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("The gateway was not able to be removed from the Avb Tags Cloud.  No changes have been made.", ex.Message, true);

			_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);
			_storeCredentialService.Verify(x => x.GetById(currentStoreId), Times.Once);
			_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Never);
			_gatewayStoreService.Verify(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()), Times.Never);

			throw;
		}
	}

	[TestMethod]
	[ExpectedException(typeof(Exception))]
	public async Task MoveToNewStore_StandardMoveAddFailed_ExpectedError()
	{
		int currentStoreId = 9;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentStoreId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => null);
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_avbTagsGatewayCommClient.Setup(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 400 });
		_avbTagsGatewayCommClient.Setup(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_gatewayStoreService.Setup(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()))
			.ReturnsAsync(() => new GatewayStoreModel() { GatewayId = currentGatewayId, StoreId = currentStoreId });


		try
		{
			var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);
		}
		catch (Exception ex)
		{
			Assert.AreEqual("Failed to restore gateway to original store location in the Avb Tags Cloud.", ex.Message, true);

			_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);
			_storeCredentialService.Verify(x => x.GetById(currentStoreId), Times.Once);
			_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Exactly(2));
			_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);
			_avbTagsGatewayCommClient.Verify(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Never);
			_gatewayStoreService.Verify(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()), Times.Once);

			throw;
		}
	}

	[TestMethod]
	public async Task MoveToNewStore_GatewayInMultipleCloudStores_Success()
	{
		int currentStoreId = 9;
		int newStoreId = 3294;
		int currentGatewayId = 132;
		var gatewayMacAddress = "AC233FC1A3D7";
		var gatewayName = "MyGateway-Somewhere";
		var currentAvbTagsStoreId = "1855990066111787008";
		var newAvbTagsStoreId = "1889337675847770112";


		_storeCredentialService.Setup(x => x.GetById(newStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = newStoreId, AvbTagsStoreId = newAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_storeCredentialService.Setup(x => x.GetById(currentStoreId))
			.Returns(() => new StoreCredentialModel() { StoreId = currentStoreId, AvbTagsStoreId = currentAvbTagsStoreId, Username = "protest1apiuser", Password = "e87a2c3f078720be7a0925a69f897afe" });
		_gatewayService.Setup(x => x.GetById(currentGatewayId))
			.Returns(() => new GatewayModel() { GatewayId = currentGatewayId, StoreId = currentStoreId, MacAddress = gatewayMacAddress, Name = gatewayName, AvbTagsGatewayId = "68b20b8aa06287b32add51c7" });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "68b20b8aa06287b32add51c7", Name = gatewayName, Mac = gatewayMacAddress, StoreId = currentAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModelData() { Id = "78b20b8aa06287b32add51c6", Name = gatewayName, Mac = gatewayMacAddress, StoreId = newAvbTagsStoreId });
		_avbTagsGatewayCommClient.Setup(x => x.RemoveGateway(gatewayMacAddress, It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_avbTagsGatewayCommClient.Setup(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId))
			.ReturnsAsync(() => new AvbTagsGatewayResponseModel() { Code = 200 });
		_gatewayStoreService.Setup(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()))
			.ReturnsAsync(() => new GatewayStoreModel() { GatewayId = currentGatewayId, StoreId = newStoreId });


		var response = await _gatewayControllerService.MoveToNewStore(currentGatewayId, newStoreId);

		Assert.AreEqual(true, response.Success);
		Assert.AreEqual("Gateway / Store association has been updated successfully in the data store.", response.Message, true);

		_storeCredentialService.Verify(x => x.GetById(newStoreId), Times.Once);
		_storeCredentialService.Verify(x => x.GetById(currentStoreId), Times.Once);
		_gatewayService.Verify(x => x.GetById(currentGatewayId), Times.Once);
		_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), currentAvbTagsStoreId), Times.Once);
		_avbTagsGatewayCommClient.Verify(x => x.GetGateway(gatewayMacAddress, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Once);
		_avbTagsGatewayCommClient.Verify(x => x.AddGateway(gatewayMacAddress, gatewayName, It.IsAny<string>(), It.IsAny<string>(), newAvbTagsStoreId), Times.Never);
		_gatewayStoreService.Verify(x => x.UpdateAsync(It.IsAny<GatewayStoreModel>()), Times.Once);
	}
}
