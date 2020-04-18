using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Service.Interface;
using CMA.ISMAI.Sagas.Service.Model;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class CreditacaoDomainService : ICreditacaoDomainService
    {
        private readonly ISagaService _creditacoesService;

        public CreditacaoDomainService(ISagaService creditacoesService)
        {
            _creditacoesService = creditacoesService;
        }

        public string CreateNewCard(string cardId, string courseName, string studentName, string courseInstitute, DateTime dueTime, bool IsCetOrOtherCondition, int boardId)
        {
            if (!getCardStatus(cardId))
                return string.Empty;
            List<string> filesUrl = getCardAttachments(cardId);
            string newCardId = _creditacoesService.PostNewCard(new CardDto($"{courseInstitute} - {courseName} - {studentName}",
                dueTime, $"{courseInstitute} - {courseName} - {studentName} - A new card has been created. When this task is done, please check it has done",
                boardId,
                filesUrl, courseInstitute, courseName, studentName, IsCetOrOtherCondition));
            if (string.IsNullOrEmpty(newCardId))
                return string.Empty;
            return newCardId;
        }
        private bool getCardStatus(string cardId)
        {
            bool getCardStatus = _creditacoesService.GetCardState(cardId);
            return getCardStatus;
        }

        private List<string> getCardAttachments(string cardId)
        {
            return _creditacoesService.GetCardAttachments(cardId);
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
    }
}
