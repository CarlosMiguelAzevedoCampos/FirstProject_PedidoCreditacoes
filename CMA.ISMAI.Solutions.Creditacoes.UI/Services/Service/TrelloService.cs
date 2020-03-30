using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service
{
    public class TrelloService : ITrelloService
    {
        private readonly IHttpRequest _request;
        public TrelloService(IHttpRequest request)
        {
            this._request = request;
        }
        public string CreateTrelloCard(CreditacaoDto creditacaoDto)
        {
            string cardName = $"{creditacaoDto.InstituteName} - {creditacaoDto.CourseName} - {creditacaoDto.StudentName}";
            string cardDescription = $"{creditacaoDto.InstituteName} - {creditacaoDto.CourseName} - {creditacaoDto.StudentName} - A new card has been created. When this task is done, please check it has done";
            var card = new CardDto(cardName, DateTime.Now.AddDays(1), cardDescription, 0, new List<string>() { creditacaoDto.Documents });
            string value =_request.PostNewCardAsync(card).Result;
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value; 
        }
    }
}
