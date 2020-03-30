using CMA.ISMAI.Solutions.Creditacoes.UI.Models;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface
{
    public interface IWorkFlowService
    {
        bool CreateWorkFlowProcess(CreditacaoDto creditacaoDto, string cardId);
    }
}
