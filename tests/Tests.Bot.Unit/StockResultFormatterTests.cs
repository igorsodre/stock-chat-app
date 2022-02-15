using FluentAssertions;
using StockChatApp.Bot.Models;
using StockChatApp.Bot.Services;
using Xunit;

namespace Tests.Bot.Unit;

public class StockResultFormatterTests
{
    private readonly StockResultFormatter _sut = new StockResultFormatter();

    [Fact]
    public void FormatStock_ReturnCorrectFormattedString()
    {
        // Arrange
        var stock = new StockResult
        {
            Symbol = "TestSymbol",
            Open = 29.50M
        };

        // Act
        var result = _sut.FormatStock(stock);

        // Assert
        result.Should().Be("TestSymbol quote is $29.50 per share");
    }
}
