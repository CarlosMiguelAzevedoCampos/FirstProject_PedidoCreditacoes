using CMA.ISMAI.Trello.Engine.Interface;
using Manatee.Trello;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Engine.Service
{
    public class TrelloService : ITrello
    {
        private readonly Logging.Interface.ILog _log;
        private readonly TrelloFactory _factory;

        public TrelloService(Logging.Interface.ILog log)
        {
            _log = log;
            _factory = new TrelloFactory();
            TrelloAuthorization.Default.AppKey = "b9ef9c087e54b015072af32ee9678bbe";
            TrelloAuthorization.Default.UserToken = "0aa0946c32a9925cbcbf5125c4a6db676061502adde9b8213fc0e9059f59f9e9";
        }

        public async Task<string> AddCard(string name, string description, DateTime dueDate)
        {
            try
            {
                var board = _factory.Board("5e6e76819bd75b48b5f5a9ec");
                await board.Refresh();
                var list = board.Lists.First();
                var newCard = await list.Cards.Add(name, description,null,dueDate);
                this._log.Info("A new card has created in trello!");
                return newCard.Id;
            }
            catch (Exception ex)
            {
                this._log.Fatal(ex.ToString());
            }
            return string.Empty;
        }

        public async Task<int> IsTheProcessFinished(string cardId)
        {
            try
            {
                var card = _factory.Card(cardId);
                await card.Refresh();
                bool result = card.IsComplete ?? false;
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                this._log.Fatal(ex.ToString());
            }
            return 3;
        }
    }
}
