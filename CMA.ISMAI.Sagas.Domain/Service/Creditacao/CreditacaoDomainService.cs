using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Service.Interface;
using CMA.ISMAI.Sagas.Service.Model;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Service.Creditacao
{
    public class CreditacaoDomainService : ICreditacaoDomainService
    {
        private readonly ISagaService _creditacoesService;

        public CreditacaoDomainService(ISagaService creditacoesService)
        {
            _creditacoesService = creditacoesService;
        }

        public string CreateNewCard(string cardId, string description, string courseName, string studentName, string courseInstitute, DateTime dueTime, bool IsCetOrOtherCondition, int boardId)
        {
            List<string> filesUrl = GetCardAttachments(cardId);
            string newCardId = _creditacoesService.PostNewCard(new CardDto($"{courseInstitute} - {courseName} - {studentName}",
                dueTime, $"{courseInstitute} - {courseName} - {studentName} - {description}",
                boardId,
                filesUrl, courseInstitute, courseName, studentName, IsCetOrOtherCondition));
            if (string.IsNullOrEmpty(newCardId))
                return string.Empty;
            return newCardId;
        }

        public List<string> GetCardAttachments(string cardId)
        {
            return _creditacoesService.GetCardAttachments(cardId);
        }

        public bool GetCardStatus(string cardId)
        {
            return _creditacoesService.GetCardState(cardId);
        }

        public bool IsSummerBreakTime(int month)
        {
            if (!IsSummerBreakActivated())
                return false;
            return month == 8;
        }

        private bool IsSummerBreakActivated()
        {
            return _creditacoesService.IsSummerBreakTime();
        }

        public bool DeleteCard(string cardId)
        {
            return _creditacoesService.DeleteCard(cardId);
        }

        public DateTime AddWorkingDays(DateTime date, int days)
        {
            while (days > 0)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek < DayOfWeek.Saturday &&
                    date.DayOfWeek > DayOfWeek.Sunday)
                    days--;
            }
            return date;
        }
    }
}
