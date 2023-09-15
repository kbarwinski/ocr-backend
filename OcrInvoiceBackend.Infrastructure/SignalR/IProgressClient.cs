namespace OcrInvoiceBackend.Infrastructure.SignalR
{
    public interface IProgressClient
    {
        Task ReceiveMessage(string message);
    }
}