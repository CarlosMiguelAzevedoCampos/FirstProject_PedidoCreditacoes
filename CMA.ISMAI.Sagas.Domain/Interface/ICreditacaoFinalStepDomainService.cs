using CamundaClient.Dto;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ICreditacaoFinalStepDomainService
    {
        bool FinishProcess(string processName, ExternalTask externalTask);
    }
}
