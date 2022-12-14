using CamundaClient.Dto;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ICreditacaoWithNoCardCreationDomainService
    {
        bool ValidCardStateAndFinishProcess(string processName, ExternalTask externalTask);
    }
}
