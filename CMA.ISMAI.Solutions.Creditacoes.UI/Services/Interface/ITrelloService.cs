using CMA.ISMAI.Solutions.Creditacoes.UI.Models;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services
{
    public interface ITrelloService
    {
        bool CreateTrelloCard(CreditacaoDto creditacaoDto);
    }
}
