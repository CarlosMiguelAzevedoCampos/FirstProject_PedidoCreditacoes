using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Services.Interface
{
    public interface ICreditacaoService
    {
        string CreditacaoWithNewCardCreation(string cardId, string courseName, string studentName, string courseInstitute,
            DateTime dueTime, bool isCet, int boardId);
        bool CreditacaoWithNoCardCreation(string cardId);
        List<string> GetCardAttachments(string cardId);
    }
}
