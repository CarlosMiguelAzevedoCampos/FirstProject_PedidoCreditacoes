using CamundaClient.Dto;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ICreditacaoFinalStepDomain
    {
        bool FinishProcess(string processName, ExternalTask externalTask);
    }
}
