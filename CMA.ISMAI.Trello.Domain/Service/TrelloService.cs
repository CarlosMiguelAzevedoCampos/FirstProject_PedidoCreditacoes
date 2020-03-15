using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Domain.Model;
using CMA.ISMAI.Trello.Engine.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.Service
{
    public class TrelloService : ITrelloService
    {
        private readonly ITrello _trello;
        private readonly ILog _log;

        public TrelloService(ITrello trello, ILog log)
        {
            _trello = trello;
            _log = log;
        }

        public Task<string> AddCard(Card card)
        {
            if (!card.IsValid(card))
            {
                _log.Fatal("A invalid card was been submited in the Domain");
                return Task.FromResult(string.Empty);
            }
            string cardId =_trello.AddCard(card.Name, card.Description, card.DueTime).Result;
            return Task.FromResult(cardId);
        }

        public Task<bool> IsTheProcessFinished(string cardId)
        {
            bool result = _trello.IsTheProcessFinished(cardId).Result;
            return Task.FromResult(result);
        }
    }
}
