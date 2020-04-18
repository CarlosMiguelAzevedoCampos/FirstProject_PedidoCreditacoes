using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ICreditacaoDomainService
    {
        string CreateNewCard(string cardId, string courseName, string studentName, string courseInstitute,
            DateTime dueTime, bool IsCetOrOtherCondition, int boardId);
        bool GetCardStatus(string cardId);
        List<string> GetCardAttachments(string cardId);
        bool IsSummerBreakTime(int month);
        bool DeleteCard(string cardId);
    }
}
