namespace StockChatApp.Bot.Models.Commands;

public class GetStockDto
{
    public string StockCode { get; set; }

    public string Channel { get; set; }

    public string ConnectionId { get; set; }
}
