using CamundaClient.Dto;
using System;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ICreditacaoWithCardCreationDomainService 
    {
        bool CreateCardAndFinishProcess(string processName, ExternalTask externalTask, int boardId, DateTime dueTime, bool IsCetOrOtherCondition = false);
    }
}
