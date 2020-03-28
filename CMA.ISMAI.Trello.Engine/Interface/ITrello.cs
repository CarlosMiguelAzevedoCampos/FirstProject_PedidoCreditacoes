using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Engine.Interface
{
    public interface ITrello
    {
        Task<string> AddCard(string name, string description, DateTime dueDate, int boardId, List<string> filesUrl);

        Task<int> IsTheProcessFinished(string cardId);
        Task<List<string>> ReturnCardAttachmenets(string cardId);
    }
}
