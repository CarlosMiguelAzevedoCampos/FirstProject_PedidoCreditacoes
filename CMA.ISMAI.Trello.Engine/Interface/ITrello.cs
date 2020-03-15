using System;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Engine.Interface
{
    public interface ITrello
    {
        Task<string> AddCard(string name, string description, DateTime dueDate);

        Task<bool> IsTheProcessFinished(string cardId);
    }
}
