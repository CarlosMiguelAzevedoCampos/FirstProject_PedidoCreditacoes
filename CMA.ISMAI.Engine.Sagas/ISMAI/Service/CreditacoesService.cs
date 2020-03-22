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
                string createCard = _httpRequest.CardPostAsync("CoordenatorExcelAction", DateTime.Now.AddDays(1), 1, "CoordenatorExcelAction").Result;
                return createCard;
            }
            return string.Empty;
        }


        public string CientificVerifiesCreditions(string cardId, string files)
        {
            _log.Info($"CientificVerifiesCreditions for card Id {cardId}");
            bool getCardState = _httpRequest.CardStateAsync(cardId).Result;
            _log.Info($"CientificVerifiesCreditions for card Id {cardId} - the result was {getCardState.ToString()}");
            if (getCardState)
            {
                // Create new card
                string createCard = _httpRequest.CardPostAsync("CientificVerifiesCreditions", DateTime.Now.AddDays(1), 1, "CientificVerifiesCreditions").Result;
                return createCard;
            }
            return string.Empty;
        }


        public string DepartamentVerifyProcess(string cardId, string files)
        {
            _log.Info($"DepartamentVerifyProcess for card Id {cardId}");
            bool getCardState = _httpRequest.CardStateAsync(cardId).Result;
            _log.Info($"DepartamentVerifyProcess for card Id {cardId} - the result was {getCardState.ToString()}");
            if (getCardState)
            {
                // Create new card
                string createCard = _httpRequest.CardPostAsync("DepartamentVerifyProcess", DateTime.Now.AddDays(1), 1, "DepartamentVerifyProcess").Result;
                return createCard;
            }
            return string.Empty;
        }

        public string PublishResult(string cardId, string files)
        {
            _log.Info($"PublishResult for card Id {cardId}");
            bool getCardState = _httpRequest.CardStateAsync(cardId).Result;
            _log.Info($"PublishResult for card Id {cardId} - the result was {getCardState.ToString()}");
            if (getCardState)
            {
                // Create new card
                string createCard = _httpRequest.CardPostAsync("Publish Result", DateTime.Now.AddDays(1), 1, "LolxD").Result;
                return createCard;
            }
            return string.Empty;
        }
    }
}
