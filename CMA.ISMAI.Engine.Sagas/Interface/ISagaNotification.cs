namespace CMA.ISMAI.Sagas.Service.Interface
{
    public interface ISagaNotification
    {
        void SendNotification(string to, string text);
    }
}
