namespace CMA.ISMAI.Trello.MessageBroker.Interface
{
    public interface ISendNotificationService
    {
        void SendNotificationToBroker(string to, string text);
    }
}
