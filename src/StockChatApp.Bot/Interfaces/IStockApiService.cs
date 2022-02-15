namespace StockChatApp.Bot.Interfaces;

public interface IStockApiService
{
    Task<string> GetStockCsvStringAsync(string stockCode);
}
