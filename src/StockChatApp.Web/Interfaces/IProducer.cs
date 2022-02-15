namespace StockChatApp.Web.Interfaces;

public interface IProducer : IDisposable
{
    void ProduceMessage(string arguments, string connectionId);
}
