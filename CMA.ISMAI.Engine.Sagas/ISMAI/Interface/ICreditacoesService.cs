namespace CMA.ISMAI.Engine.Automation.Sagas
{
    public interface ICreditacoesService
    {
        string CoordenatorExcelAction(string cardId, string files);
        string DepartamentVerifyProcess(string cardId, string files);
        string CientificVerifiesCreditions(string cardId, string files);
        string PublishResult(string cardId, string files);
    }
}
