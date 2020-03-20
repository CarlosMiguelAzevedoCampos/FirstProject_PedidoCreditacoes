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
        public bool CoordenatorExcelAction(string cardId, string files)
        {
            _log.Info($"CoordenatorExcelAction for card Id {cardId}");
            bool result = _httpRequest.CardStateAsync(cardId).Result;
            if (result)
            {
                CreateCompleteProcess(cardId);
                return true;
            }
            NotifyUser(cardId);
            _log.Info($"CoordenatorExcelAction for card Id {cardId} - the result was {result.ToString()}");
            return false;
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
        private void NotifyUser(string cardId)
        {
            _log.Info($"Notify user about the card {cardId}..");

        }

        private void CreateCompleteProcess(string cardId)
        {
            _log.Info($"Process completed for cardId {cardId}!");
        }
    }
}
