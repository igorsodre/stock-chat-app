namespace StockChatApp.Bot.Interfaces;

public interface ICommandProcessor
{
    Task ProcessCommand(string message);
}
