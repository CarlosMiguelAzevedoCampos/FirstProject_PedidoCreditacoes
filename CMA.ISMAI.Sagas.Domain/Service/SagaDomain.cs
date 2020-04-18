using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Base;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class SagaDomain : TaskPolling, ISagaDomain
    {
        private readonly IDictionary<string, Action<ExternalTask>> workers;
        private readonly ILog _log;
        private readonly ICreditacaoWithCardCreationDomain _creditacaoWithCardCreation;
        private readonly ICreditacaoFinalStepDomain _creditacaoFinalStep;
        private readonly ICreditacaoWithNoCardCreationDomain _creditacaoWithNoCardCreation;
        private Timer pollingTimer;
        private readonly int _pollingtime;

        public SagaDomain(ILog log, ICreditacaoFinalStepDomain creditacaoFinalStep, ICreditacaoWithNoCardCreationDomain creditacaoWithNoCardCreation, ICreditacaoWithCardCreationDomain creditacaoWithCardCreation) : base(log)
        {
            _log = log;
            _creditacaoWithCardCreation = creditacaoWithCardCreation;
            _creditacaoFinalStep = creditacaoFinalStep;
            _creditacaoWithNoCardCreation = creditacaoWithNoCardCreation;
            workers = new Dictionary<string, Action<ExternalTask>>();
            _pollingtime = 30000;
        }
        public void RegistWorkers()
        {
            registerWorker("course-coordinator", externalTask =>
            {
                _log.Info($"Course coordinator task non-cet is running..{externalTask.Id} -{DateTime.Now}");
                Console.WriteLine($"Course coordinator  non-cet task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "course-coordinator")));
                _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 1, dueTime);
            });

            registerWorker("department-director", externalTask =>
            {
                Console.WriteLine($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "department-director")));
                _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 2, dueTime);
            });
            registerWorker("scientific-council", externalTask =>
            {
                Console.WriteLine($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                _creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("CreditacaoISMAI", externalTask);
            });

            registerWorker("coordenator-jury", externalTask =>
            {
                Console.WriteLine($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "coordenator-jury")));
                _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 0, dueTime, true);
            });

            registerWorker("jury-delibers", externalTask =>
            {
                Console.WriteLine($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "jury-delibers")));
                _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 2, dueTime, true);
            });

            registerWorker("presidentcouncil-evaluates", externalTask =>
            {
                Console.WriteLine($"Councilium president evaluates task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Councilium president evaluates task is running..{externalTask.Id} -{DateTime.Now}");
                _creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("CreditacaoISMAI", externalTask);
            });

            registerWorker("final-result", externalTask =>
            {
                Console.WriteLine($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                _creditacaoFinalStep.FinishProcess("CreditacaoISMAI", externalTask);
            });

            pollingTimer = new Timer(_ => StartPollCreditacoesTask("CreditacaoISMAI"), null, _pollingtime, Timeout.Infinite);
        }

        private void StartPollCreditacoesTask(string workerId)
        {
            _log.Info($"Time to poll tasks!, workerId is {workerId}");
            Console.WriteLine($"Time to poll tasks!, workerId is {workerId} - {DateTime.Now}");
            PollTasks(workerId, workers);
            pollingTimer.Change(_pollingtime, Timeout.Infinite);
        }

        private void registerWorker(string topicName, Action<ExternalTask> action)
        {
            workers.Add(topicName, action);
        }
    }
}
