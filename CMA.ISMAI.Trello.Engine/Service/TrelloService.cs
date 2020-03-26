using CMA.ISMAI.Trello.Engine.Enum;
using CMA.ISMAI.Trello.Engine.Interface;
using Manatee.Trello;
using System;
using System.Collections.Generic;
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

        public async Task<string> AddCard(string name, string description, DateTime dueDate, int boardId, List<string> filesUrl)
        {
            try
            {
                string boardIdentifier = GetBoardId(boardId);
                if (VerifyBoardIdentifier(boardIdentifier))
                    return string.Empty;

                var board = _factory.Board(GetBoardId(boardId));
                await board.Refresh();
                var list = board.Lists.FirstOrDefault();
                var newCard = await list.Cards.Add(name, description, null, dueDate);
                var tasks = AddAttachmentsToACardAsync(newCard, filesUrl);
                await Task.WhenAll(tasks);
                this._log.Info("A new card has created in trello!");
                return newCard.Id;
            }
            catch (Exception ex)
            {
                this._log.Fatal(ex.ToString());
            }
            return string.Empty;
        }

        private async Task AddAttachmentsToACardAsync(ICard newCard, List<string> filesUrl)
        {
            foreach (var item in filesUrl)
            {
                await newCard.Attachments.Add(item, "Documents");
            }
        }

        private bool VerifyBoardIdentifier(string id)
        {
            return string.IsNullOrEmpty(id);
        }

        private string GetBoardId(int boardId)
        {
            switch (boardId)
            {
                case (int)BoardType.Course_coordinator:
                    return "5e7a615492c8e839429fd13b";
                case (int)BoardType.Department_director:
                    return "5e7a61697c32d815f76c71ce";
                case (int)BoardType.Scientific_council:
                    return "5e7a617c1c87ae276116c00f";
                default:
                    return string.Empty;
            }
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

        public async Task<List<string>> ReturnCardAttachmenets(string cardId, int boardId)
        {
            List<string> filesUrl = new List<string>();
            var card = _factory.Card(cardId);
            await card.Refresh();
            if (card != null)
            {
                foreach (var item in card.Attachments)
                {
                    filesUrl.Add(item.Url);
                }
            }
            return filesUrl;
        }
    }
}
