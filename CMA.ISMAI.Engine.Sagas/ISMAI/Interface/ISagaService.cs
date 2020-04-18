using CMA.ISMAI.Sagas.Service.Model;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Service.Interface
{
    public interface ISagaService
    {
        bool GetCardState(string cardId);
        string PostNewCard(CardDto card);
        List<string> GetCardAttachments(string cardId);
        bool IsSummerBreakTime();
    }
}
