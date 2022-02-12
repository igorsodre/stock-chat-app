using StockChatApp.Bot.Models;

namespace StockChatApp.Bot.Interfaces;

public interface IStockResultFormatter
{
    string FormatStock(StockResult stock);
}
