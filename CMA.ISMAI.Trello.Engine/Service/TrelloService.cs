using CMA.ISMAI.Core;
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
            TrelloAuthorization.Default.AppKey = GetAppKey();
            TrelloAuthorization.Default.UserToken = GetUserToken();
        }

        public async Task<string> AddCard(string name, string description, DateTime dueDate, int boardId, List<string> filesUrl, string username)
        {
            try
            {
                string boardIdentifier = GetBoardId(boardId);
                if (VerifyBoardIdentifier(boardIdentifier))
                    return string.Empty;

                var board = _factory.Board(GetBoardId(boardId));
                await board.Refresh();
                IEnumerable<IMember> members = ObainUserForTheCard(username, board);
                var list = board.Lists.FirstOrDefault();
                var newCard = await list.Cards.Add(name, description, null, dueDate, null, members);
                var tasks = AddAttachmentsToACard(newCard, filesUrl);
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

        private static IEnumerable<IMember> ObainUserForTheCard(string username, IBoard board)
        {
            var member = board.Memberships;
            int userPosition = member.ToList().FindIndex(x => x.Member.UserName == username);
            List<IMember> mem = new List<IMember>();
            mem.Add(member[userPosition].Member);
            IEnumerable<IMember> members = mem;
            return members;
        }

        private async Task AddAttachmentsToACard(ICard newCard, List<string> filesUrl)
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
                    return GetCoordinatorBoardId();
                case (int)BoardType.Department_director:
                    return GetDepartmentDirectorBoardId();
                case (int)BoardType.Scientific_council:
                    return GetScientificCouncilBoardId();
                case (int)BoardType.Testing_board:
                    return GetTestingBoardId();
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

        public async Task<List<string>> ReturnCardAttachmenets(string cardId)
        {
            try
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
            catch (Exception ex)
            {
                this._log.Fatal(ex.ToString());
            }
            return null;
        }

        public async Task<bool> DeleteCard(string id)
        {
            try
            {
                var card = _factory.Card(id);
                await card.Refresh();
                await card.Delete();
                await card.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                this._log.Fatal(ex.ToString());
            }
            return false;
        }

        private string GetAppKey() => BaseConfiguration.ReturnSettingsValue("TrelloKey", "AppKey");
        private string GetUserToken() => BaseConfiguration.ReturnSettingsValue("TrelloKey", "UserToken");
        private string GetCoordinatorBoardId() => BaseConfiguration.ReturnSettingsValue("BoardIds", "Course_coordinator");
        private string GetDepartmentDirectorBoardId() => BaseConfiguration.ReturnSettingsValue("BoardIds", "Department_director");
        private string GetScientificCouncilBoardId() => BaseConfiguration.ReturnSettingsValue("BoardIds", "Scientific_council");
        private string GetTestingBoardId() => BaseConfiguration.ReturnSettingsValue("BoardIds", "Testing_board");
    }
}
