using RepositroyContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Xml.Linq;
using Moq;
using FluentAssertions;
using Entities;
using AutoFixture;
namespace Tests.ServiceTests
{
  public class StocksServiceTest
  {
    private readonly IStocksService _stocksService;
    private readonly IStocksRepository _stocksRepository;
    private readonly Mock<IStocksRepository> _stocksRepositoryMock;
    private readonly IFixture _fixture;
    public StocksServiceTest()
    {
      _fixture = new Fixture();
      _stocksRepositoryMock = new Mock<IStocksRepository>();
      _stocksRepository = _stocksRepositoryMock.Object;
      _stocksService = new StocksService(_stocksRepository);
    }



    #region CreateBuyOrder


    [Fact]
    public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = null;

      //Act
      var action = async () => await _stocksService.CreateBuyOrder(buyOrderRequest);
      //Assert
      await action.Should().ThrowAsync<ArgumentNullException>();
    }



    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(0)] //passing parameters to the test method
    public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

      //Act
      var action = async () => await _stocksService.CreateBuyOrder(buyOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(100001)] //passing parameters to the test method
    public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

      //Act
      var action = async () => await _stocksService.CreateBuyOrder(buyOrderRequest);
      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(0)] //passing parameters to the test method
    public async Task CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };

      //Act
      var action = async () => await _stocksService.CreateBuyOrder(buyOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(10001)] //passing parameters to the test method
    public async Task CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

      //Act
      var action =async () => await _stocksService.CreateBuyOrder(buyOrderRequest);
      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

      //Act
      var action = async () => await _stocksService.CreateBuyOrder(buyOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

      //Act
      //Assert.Throws<ArgumentException>(() =>
      //{
      //  _stocksService.CreateBuyOrder(buyOrderRequest);
      //});
      var action = async () => await _stocksService.CreateBuyOrder(buyOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };
      BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
      BuyOrderResponse expected_buyOrder = buyOrder.ToBuyOrderResponse();
      _stocksRepositoryMock.Setup(temp => temp.AddBuyOrder(It.IsAny<BuyOrder>()))
        .ReturnsAsync((BuyOrder c) => c);

      //Act
      BuyOrderResponse actual_buyOrder = await _stocksService.CreateBuyOrder(buyOrderRequest);
      expected_buyOrder.BuyOrderID = actual_buyOrder.BuyOrderID;
      //Assert
      actual_buyOrder.Should().BeEquivalentTo(expected_buyOrder);
    }


    #endregion


    

    #region CreateSellOrder


    [Fact]
    public async Task CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = null;

      //Act
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);

      //Assert
      await action.Should().ThrowAsync<ArgumentNullException>();
    }



    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(0)] //passing parameters to the test method
    public async Task CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

      //Act
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(100001)] //passing parameters to the test method
    public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

      //Act
      //Assert.Throws<ArgumentException>(() =>
      //{
      //  _stocksService.CreateSellOrder(sellOrderRequest);
      //});
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);
      //Assert
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(0)] //passing parameters to the test method
    public async Task CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = sellOrderPrice, Quantity = 1 };

      //Act
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
    [InlineData(10001)] //passing parameters to the test method
    public async Task CreateSellOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

      //Act
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

      //Act
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

      //Act
      var action = async () => await _stocksService.CreateSellOrder(sellOrderRequest);
      await action.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task CreateSellOrder_ValidData_ToBeSuccessful()
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };
      SellOrder sellOrder = sellOrderRequest.ToSellOrder();
      SellOrderResponse expected_sellOrder = sellOrder.ToSellOrderResponse();
      _stocksRepositoryMock.Setup(temp => temp.AddSellOrder(It.IsAny<SellOrder>()))
        .ReturnsAsync((SellOrder s) => s);
      //Act
      SellOrderResponse actual_sellOrder = await _stocksService.CreateSellOrder(sellOrderRequest);
      expected_sellOrder.SellOrderID = actual_sellOrder.SellOrderID;
      //Assert
      actual_sellOrder.Should().BeEquivalentTo(expected_sellOrder);
    }


    #endregion




    #region GetBuyOrders

    //The GetAllBuyOrders() should return an empty list by default
    [Fact]
    public async Task GetAllBuyOrders_DefaultList_ToBeEmpty()
    {
      _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
        .ReturnsAsync(new List<BuyOrder>());
      //Act
      List<BuyOrderResponse> actual_listfromGetBuyOrders = await _stocksService.GetBuyOrders();

      //Assert
      actual_listfromGetBuyOrders.Should().BeEmpty();
    }


    [Fact]
    public async Task GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
    {
      //Arrange

      //Create a list of buy orders with hard-coded data
      //BuyOrderRequest buyOrder_request_1 = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateAndTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

      //BuyOrderRequest buyOrder_request_2 = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateAndTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

      //List<BuyOrderRequest> buyOrder_requests = new List<BuyOrderRequest>() { buyOrder_request_1, buyOrder_request_2 };

      //List<BuyOrderResponse> buyOrder_response_list_from_add = new List<BuyOrderResponse>();
      List<BuyOrder> buyOrders = _fixture.Create<List<BuyOrder>>();
      List<BuyOrderResponse> expected_buyOrders = buyOrders.Select(b => b.ToBuyOrderResponse()).ToList();

      _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
        .ReturnsAsync(buyOrders);
      //Act
      List<BuyOrderResponse> actual_buyOrders =await _stocksService.GetBuyOrders();


      //Assert
      actual_buyOrders.Should().BeEquivalentTo(expected_buyOrders);
    }

    #endregion




    #region GetSellOrders

    //The GetAllSellOrders() should return an empty list by default
    [Fact]
    public async Task GetAllSellOrders_DefaultList_ToBeEmpty()
    {
      _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
        .ReturnsAsync(new List<SellOrder>());
      //Act
      List<SellOrderResponse> actual_sellOrders =await _stocksService.GetSellOrders();

      //Assert
      actual_sellOrders.Should().BeEmpty();
    }


    [Fact]
    public async Task GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
    {
      //Arrange
      List<SellOrder> sellOrders = _fixture.Create<List<SellOrder>>();
      List<SellOrderResponse> expected_sellOrders = sellOrders.Select(s => s.ToSellOrderResponse()).ToList();

      _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
        .ReturnsAsync(sellOrders);
      //Act
      List<SellOrderResponse> actual_sellOrders = await _stocksService.GetSellOrders();


      //Assert
      actual_sellOrders.Should().BeEquivalentTo(expected_sellOrders);
    }

    #endregion
    
  }
}