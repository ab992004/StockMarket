using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tests.IntegrationTests
{
  public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
  {
    private readonly HttpClient _client;

    public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
      _client = factory.CreateClient();
    }


    #region Index
    // This test requires a valid Finnhub API token configured outside User Secrets and appsettings.Development,
    // as these configuration sources are not loaded in the test environment.

    [Fact]
    public async Task Index_ToReturnView()
    {
      //Arrange

      //Act
      HttpResponseMessage response = await _client.GetAsync("/Trade/Index/MSFT");

      //Assert
      response.IsSuccessStatusCode.Should().BeTrue(); 

      string responseBody = await response.Content.ReadAsStringAsync();

      HtmlDocument html = new HtmlDocument();
      html.LoadHtml(responseBody);
      var document = html.DocumentNode;

      document.QuerySelectorAll(".price").Should().NotBeNull();
    }

    #endregion
  }
}