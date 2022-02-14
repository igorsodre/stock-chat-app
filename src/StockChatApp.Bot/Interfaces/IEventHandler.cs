namespace StockChatApp.Bot.Interfaces;

public interface IEventHandler
{
    Task ProcessEvent(string message);
}
