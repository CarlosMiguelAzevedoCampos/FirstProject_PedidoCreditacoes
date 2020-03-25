using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface
{
    public interface IHttpRequest
    {
        Task<bool> CardStateAsync(string cardId);
        Task<List<string>> GetCardAttachments(string cardId, int boardId);
        Task<string> CardPostAsync(CardDto card);
    }
}
