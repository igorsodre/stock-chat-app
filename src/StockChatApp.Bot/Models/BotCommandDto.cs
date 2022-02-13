namespace StockChatApp.Bot.Models;

public class BotCommandDto<T> where T : class, new()
{
    public string Command { get; set; }

    public T Data { get; set; }
}
