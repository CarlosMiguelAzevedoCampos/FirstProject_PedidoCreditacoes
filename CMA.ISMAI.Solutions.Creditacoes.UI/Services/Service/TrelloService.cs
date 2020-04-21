using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service
{
    public class TrelloService : ITrelloService
    {
        private readonly IHttpRequest _request;
        private readonly ILog _log;
        public TrelloService(IHttpRequest request, ILog log)
        {
            this._request = request;
            this._log = log;
        }
        public bool CreateTrelloCard(CreditacaoDto creditacaoDto)
        {
            string cardName = $"{creditacaoDto.InstituteName} - {creditacaoDto.CourseName} - {creditacaoDto.StudentName}";
            string cardDescription = ReturnCardDescriptionBasedOnTheChoseenProcess(creditacaoDto.IsCetOrOtherCondition);
            var card = new CardDto(cardName, CreateDueTime(creditacaoDto.IsCetOrOtherCondition ? Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "course-coordinator-jury")) : Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "course-coordinator-proposal"))) , cardDescription, 0, new List<string>() { creditacaoDto.Documents },
                creditacaoDto.InstituteName, creditacaoDto.CourseName, creditacaoDto.StudentName, creditacaoDto.IsCetOrOtherCondition);
            _log.Info("Posting a new card to trello...");
            bool value = _request.PostNewCardAsync(card).Result;
            return value;
        }

        private string ReturnCardDescriptionBasedOnTheChoseenProcess(bool isCetOrOtherCondition)
        {
            return isCetOrOtherCondition ? "O Coordenador de Curso convoca o Júri" : "O coordenador de curso deve prodecer à proposta de creditação, incluido o preechimento da tabela e/ou matriz de creditação, ouvindo, se necessário, os regentes das unidades curriculares.";
        }

        private DateTime CreateDueTime(int days)
        {
            DateTime tmpDate = DateTime.Now;
            while (days > 0)
            {
                tmpDate = tmpDate.AddDays(1);
                if (tmpDate.DayOfWeek < DayOfWeek.Saturday &&
                    tmpDate.DayOfWeek > DayOfWeek.Sunday)
                    days--;
            }
            return tmpDate;
        }
    }
}
