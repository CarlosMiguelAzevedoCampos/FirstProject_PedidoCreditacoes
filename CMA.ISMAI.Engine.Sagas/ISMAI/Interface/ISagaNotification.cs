using CMA.ISMAI.Core.Notifications;

namespace CMA.ISMAI.Sagas.Service.Interface
{
    public interface ISagaNotification
    {
        void SendNotification(MessageBody notifications);
    }
}
