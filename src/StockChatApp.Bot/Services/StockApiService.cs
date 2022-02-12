using System.Net;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Models;
using StockChatApp.Bot.Options;

namespace StockChatApp.Bot.Services;

public class StockApiService : IStockApiService
{
    private readonly HttpClient _client;

    public StockApiService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("stockApiClient");
    }

    public async Task<string> GetStockCsvStringAsync(string stockCode)
    {
        var query = QueryString.Create(
            new[]
            {
                new KeyValuePair<string, string?>("s", stockCode),
                new KeyValuePair<string, string?>("f", "sd2t2ohlcv"),
                new KeyValuePair<string, string?>("h", ""),
                new KeyValuePair<string, string?>("e", "csv"),
            }
        );

        return await _client.GetStringAsync(query.Value);
    }
}
