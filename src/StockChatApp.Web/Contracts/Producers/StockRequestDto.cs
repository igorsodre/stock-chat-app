namespace StockChatApp.Web.Contracts.Producers;

public class StockRequestDto
{
    public string StockCode { get; set; }

    public string Channel { get; set; }

    public string ConnectionId { get; set; }
}
