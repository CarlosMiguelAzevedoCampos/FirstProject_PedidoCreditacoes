using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;
using System.Threading;

namespace CMA.ISMAI.Sagas.UI
{
    public class ConsoleApplication
    {
        private readonly ILog _log;
        private readonly ISagaDomainService _sagaCreditacoesWorker;

        public ConsoleApplication(ILog log, ISagaDomainService sagaCreditacoesWorker)
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