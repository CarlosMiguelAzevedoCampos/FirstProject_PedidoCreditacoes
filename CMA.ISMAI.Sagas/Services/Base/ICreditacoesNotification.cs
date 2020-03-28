using CMA.ISMAI.Core.Notifications;

namespace CMA.ISMAI.Sagas.Services.Base
{
    public interface ICreditacoesNotification
    {
        void SendNotification(MessageBody notifications);
    }
}
