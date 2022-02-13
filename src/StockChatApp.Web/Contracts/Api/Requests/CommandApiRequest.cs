namespace StockChatApp.Web.Contracts.Api.Requests;

public class CommandApiRequest
{
    public string Command { get; set; }

    public string Arguments { get; set; }

    public string ConnectionId { get; set; }
}
