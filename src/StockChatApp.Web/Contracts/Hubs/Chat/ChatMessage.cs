namespace StockChatApp.Web.Contracts.Hubs.Chat;

public class ChatMessage
{
    public string UserName { get; set; }

    public string Content { get; set; }

    public DateTime Timestamp { get; set; }
}
