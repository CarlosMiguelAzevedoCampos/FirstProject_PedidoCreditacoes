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
        public string CreateTrelloCard(CreditacaoDto creditacaoDto)
        {
            string cardName = $"{creditacaoDto.InstituteName} - {creditacaoDto.CourseName} - {creditacaoDto.StudentName}";
            string cardDescription = $"{creditacaoDto.InstituteName} - {creditacaoDto.CourseName} - {creditacaoDto.StudentName} - A new card has been created. When this task is done, please check it has done";
            var card = new CardDto(cardName, DateTime.Now.AddDays(1), cardDescription, 0, new List<string>() { creditacaoDto.Documents });
            _log.Info("Posting a new card to trello...");
            string value = _request.PostNewCardAsync(card).Result;

            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value;
        }
    }
}
