using StockChatApp.Bot.Models;

namespace StockChatApp.Bot.Interfaces;

public interface ICsvStockParser
{
    StockResult ParseString(string csvString);
}
