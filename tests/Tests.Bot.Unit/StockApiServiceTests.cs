using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NSubstitute;
using StockChatApp.Bot.Services;
using Xunit;

namespace Tests.Bot.Unit;

public class StockApiServiceTests
{
    private readonly IHttpClientFactory _clientFactory = Substitute.For<IHttpClientFactory>();
    private readonly HttpClient _client = Substitute.For<HttpClient>();
    private readonly Mock<HttpMessageHandler> _handlerMock = new(MockBehavior.Strict);
    private readonly StockApiService _sut;

    private const string csvString =
        "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2022-02-11,22:00:08,172.33,173.08,168.04,168.64,98670687\r\n";

    public StockApiServiceTests()
    {
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(csvString),
                }
            );
        _clientFactory.CreateClient(Arg.Any<string>())
            .Returns(new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("https://example.com") });

        _sut = new StockApiService(_clientFactory);
    }

    [Fact]
    public async Task GetStockCsvStringAsync_ReturnsValidCsvString()
    {
        var resultString = await _sut.GetStockCsvStringAsync("aapl.us");

        resultString.Should().Be(resultString);
    }
}
