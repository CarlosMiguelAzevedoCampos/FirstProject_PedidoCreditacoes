using CMA.ISMAI.Trello.Domain.Model;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface ITrelloService
    {
        Task<string> AddCard(Card card);

        Task<bool> IsTheProcessFinished(string cardId);
    }
}
