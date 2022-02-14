using System.Text.Json;
using StackExchange.Redis;
using StockChatApp.Web.Contracts.Hubs.Chat;
using StockChatApp.Web.Interfaces;
using StockChatApp.Web.Options;

namespace StockChatApp.Web.Services;

public class ChatMessagesRepository : IChatMessageRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ServerSettings _serverSettings;

    private const string MessagesKey = "9fI0JN3tdadRxWq5w7";

    public ChatMessagesRepository(IConnectionMultiplexer connectionMultiplexer, ServerSettings serverSettings)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _serverSettings = serverSettings;
    }

    // TODO: this implementation can suffer from race condition when multiple users try to send messages
    // I Only Used Redis for the purpusos of this exercise
    public async Task PostMessage(ChatMessage message)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var messagesString = await db.StringGetAsync(MessagesKey);
        var messagesList = new List<ChatMessage>();
        if (!messagesString.IsNullOrEmpty)
        {
            messagesList = JsonSerializer.Deserialize<List<ChatMessage>>(messagesString);
        }

        if (messagesList?.Count >= _serverSettings.MaxMessagesStored)
        {
            messagesList.RemoveAt(0);
        }

        messagesList.Add(message);
        await db.StringSetAsync(MessagesKey, JsonSerializer.Serialize(messagesList));
    }

    public async Task<IEnumerable<ChatMessage>> GetMessages()
    {
        var db = _connectionMultiplexer.GetDatabase();
        var messagesString = await db.StringGetAsync(MessagesKey);
        return messagesString.IsNullOrEmpty
            ? new List<ChatMessage>()
            : JsonSerializer.Deserialize<IList<ChatMessage>>(messagesString);
    }
}
