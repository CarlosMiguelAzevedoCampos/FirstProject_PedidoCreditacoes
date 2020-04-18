using CamundaClient.Dto;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ICreditacaoWithNoCardCreationDomain
    {
        void ValidCardStateAndFinishProcess(string processName, ExternalTask externalTask);
    }
}
