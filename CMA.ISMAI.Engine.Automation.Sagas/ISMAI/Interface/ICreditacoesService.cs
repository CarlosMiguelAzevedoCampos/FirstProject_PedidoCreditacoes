using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface
{
    public interface ICreditacoesService
    {
        string CoordenatorExcelAction(string cardId, string files);
        bool DepartamentVerifyProcess(string cardId, string files);
        bool CientificVerifiesCreditions(string cardId, string files);
        bool PublishResult(string cardId, string files);
    }
}
