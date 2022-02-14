using Ganss.XSS;
using Microsoft.AspNetCore.SignalR;
using StockChatApp.Web.Contracts.Hubs.Chat;
using StockChatApp.Web.Interfaces;

namespace StockChatApp.Web.Hubs;

public class ChatHub : Hub
{
    private readonly HtmlSanitizer _sanitizer = new();
    private readonly IChatMessageRepository _messageRepository;

    public ChatHub(IChatMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task SendMessage(string userName, string content)
    {
        var message = new ChatMessage
        {
            Content = _sanitizer.Sanitize(content),
            UserName = _sanitizer.Sanitize(userName),
            Timestamp = DateTime.Now
        };
        await Clients.All.SendAsync("ReceiveMessage", message);
        await _messageRepository.PostMessage(message);
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
