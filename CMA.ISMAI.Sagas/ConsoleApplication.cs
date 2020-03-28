using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Services.Base;
using System;
using System.Threading;

namespace CMA.ISMAI.Sagas
{
    public class ConsoleApplication
    {
        private readonly ILog _log;
        private readonly ISagaCreditacoesWorker _sagaCreditacoesWorker;

        public ConsoleApplication(ILog log, ISagaCreditacoesWorker sagaCreditacoesWorker)
        {
            _log = log;
            _sagaCreditacoesWorker = sagaCreditacoesWorker;
        }

        internal void Run()
        {
            _log.Info("Sagas started now!");
            Console.WriteLine("Sagas started...");
            _log.Info("Sagas startíng for CreditacaoISMAI!");
            new Thread(() => StartCreditacoesSaga()).Start();
        }

        private void StartCreditacoesSaga()
        {
            _sagaCreditacoesWorker.RegistWorkers();
        }
    }
}