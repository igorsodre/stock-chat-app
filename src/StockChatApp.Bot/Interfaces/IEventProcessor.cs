namespace StockChatApp.Bot.Interfaces;

public interface IEventProcessor
{
    Task ProcessEvent(string message);
}
