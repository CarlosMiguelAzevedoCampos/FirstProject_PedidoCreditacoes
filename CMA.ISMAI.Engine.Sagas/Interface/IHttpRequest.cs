using CMA.ISMAI.Sagas.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas.Service.Interface
{
    public interface IHttpRequest
    {
        Task<bool> CardState(string cardId);
        Task<bool> DeleteCard(string cardId);
        Task<List<string>> GetCardAttachments(string cardId);
        Task<string> CardPost(CardDto card);
    }
}
