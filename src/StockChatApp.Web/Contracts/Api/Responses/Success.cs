namespace StockChatApp.Web.Contracts.Api.Responses;

public class Success<T>
{
    public Success() { }

    public Success(T data)
    {
        Data = data;
    }

    public T Data { get; set; }
}

public class Success : Success<string>
{
    public static Success<string> Default()
    {
        return new Success<string>("OK");
    }
}
