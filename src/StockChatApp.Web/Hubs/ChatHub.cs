using Ganss.XSS;
using Microsoft.AspNetCore.SignalR;
using StockChatApp.Web.Contracts.Hubs.Chat;

namespace StockChatApp.Web.Hubs;

public class ChatHub : Hub
{
    private readonly HtmlSanitizer _sanitizer = new();

    public async Task SendMessage(string userName, string content)
    {
        await Clients.All.SendAsync(
            "ReceiveMessage",
            new ChatMessage
            {
                Content = _sanitizer.Sanitize(content),
                UserName = _sanitizer.Sanitize(userName),
                Timestamp = DateTime.Now
            }
        );
    }

    public async Task SendMessageToUser(string connectionId, string userName, string content)
    {
        await Clients.Client(connectionId)
            .SendAsync(
                "ReceiveMessage",
                new ChatMessage
                {
                    Content = _sanitizer.Sanitize(content),
                    UserName = _sanitizer.Sanitize(userName),
                    Timestamp = DateTime.Now
                }
            );
    }
}
