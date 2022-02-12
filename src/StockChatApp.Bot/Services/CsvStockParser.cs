using System.Globalization;
using CsvHelper;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Models;

namespace StockChatApp.Bot.Services;

public class CsvStockParser : ICsvStockParser
{
    public StockResult ParseString(string csvString)
    {
        using var csv = new CsvReader(new StringReader(csvString), CultureInfo.InvariantCulture);
        var records = csv.GetRecords<StockResult>();
        return records.First();
    }
}
