using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using CMA.ISMAI.Sagas.Services.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Services.Service
{
    public class CreditacaoService : ICreditacaoService
    {
        private readonly ISagaService _creditacoesService;

        public CreditacaoService(ISagaService creditacoesService)
        {
            _creditacoesService = creditacoesService;
        }

        public bool CreditacaoWithNoCardCreation(string cardId)
        {
            return getCardStatus(cardId);
        }

        public string CreditacaoWithNewCardCreation(string cardId, string courseName, string studentName, string courseInstitute, DateTime dueTime, bool isCet, int boardId)
        {
            if (!getCardStatus(cardId))
                return string.Empty;
            List<string> filesUrl = getCardAttachments(cardId);
            string newCardId = _creditacoesService.PostNewCard(new CardDto($"{courseInstitute} - {courseName} - {studentName}",
                dueTime, $"{courseInstitute} - {courseName} - {studentName} - A new card has been created. When this task is done, please check it has done",
                boardId,
                filesUrl, courseInstitute, courseName, studentName, isCet));
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
    }
}
