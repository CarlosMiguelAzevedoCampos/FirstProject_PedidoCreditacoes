using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;
using System.Threading;

namespace CMA.ISMAI.Sagas
{
    public class ConsoleApplication
    {
        private readonly ILog _log;
        private readonly ISagaDomain _sagaCreditacoesWorker;

        public ConsoleApplication(ILog log, ISagaDomain sagaCreditacoesWorker)
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