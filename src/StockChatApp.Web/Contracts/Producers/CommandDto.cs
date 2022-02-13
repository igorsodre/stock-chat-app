namespace StockChatApp.Web.Contracts.Producers;

public class CommandDto<T> where T : class, new()
{
    public string Command { get; set; }

    public T Data { get; set; }
}
