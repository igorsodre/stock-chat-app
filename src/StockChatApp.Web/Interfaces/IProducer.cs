namespace StockChatApp.Web.Interfaces;

public interface IProducer<in T> : IDisposable where T : class
{
    void ProduceMessage(T content);
}
