using CMA.ISMAI.Sagas.Engine.ISMAI.Model;

namespace CMA.ISMAI.Engine.Automation.Sagas
{
    public interface ICreditacoesService
    {
        bool GetCardState(string cardId);
        string PostNewCard(CardDto card);
    /*    string DepartamentVerifyProcess(string cardId, string files);
        string CientificVerifiesCreditions(string cardId, string files);
        string PublishResult(string cardId, string files);*/
    }
}
