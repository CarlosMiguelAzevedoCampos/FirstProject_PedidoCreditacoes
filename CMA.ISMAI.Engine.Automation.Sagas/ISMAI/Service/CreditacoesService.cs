using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Logging.Interface;
using System;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service
{
    public class CreditacoesService : ICreditacoesService
    {
        private readonly ILog _log;
        private readonly IHttpRequest _httpRequest;
        public CreditacoesService(ILog log, IHttpRequest httpRequest)
        {
            this._log = log;
            this._httpRequest = httpRequest;
        }
        public string CoordenatorExcelAction(string cardId, string files)
        {
            _log.Info($"CoordenatorExcelAction for card Id {cardId}");
            bool getCardState = _httpRequest.CardStateAsync(cardId).Result;
            _log.Info($"CoordenatorExcelAction for card Id {cardId} - the result was {getCardState.ToString()}");
            if (getCardState)
            {
                // Create new card
                string createCard = _httpRequest.CardPostAsync("", DateTime.Now.AddDays(1), "").Result;
                return createCard;
            }
            return string.Empty;
        }


        public bool CientificVerifiesCreditions(string cardId, string files)
        {
            throw new System.NotImplementedException();
        }


        public bool DepartamentVerifyProcess(string cardId, string files)
        {
            throw new System.NotImplementedException();
        }

        public bool PublishResult(string cardId, string files)
        {
            throw new System.NotImplementedException();
        }
    }
}
