using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Models;

namespace StockChatApp.Bot.Services;

public class StockResultFormatter : IStockResultFormatter
{
    public string FormatStock(StockResult stock)
    {
        return $"{stock.Symbol} quote is ${stock.Open} per share";
    }
}
