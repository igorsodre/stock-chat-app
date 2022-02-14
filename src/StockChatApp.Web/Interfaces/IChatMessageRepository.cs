using StockChatApp.Web.Contracts.Hubs.Chat;

namespace StockChatApp.Web.Interfaces;

public interface IChatMessageRepository
{
    Task PostMessage(ChatMessage message);

    Task<IEnumerable<ChatMessage>> GetMessages();
}
