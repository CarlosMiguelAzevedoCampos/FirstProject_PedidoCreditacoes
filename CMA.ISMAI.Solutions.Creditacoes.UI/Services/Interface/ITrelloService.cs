using CMA.ISMAI.Solutions.Creditacoes.UI.Models;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services
{
    public interface ITrelloService
    {
        string CreateTrelloCard(CreditacaoDto creditacaoDto);
    }
}
