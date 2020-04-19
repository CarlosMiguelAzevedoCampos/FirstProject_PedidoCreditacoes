using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas.Domain.Service.Saga
{
    public class SagaDomainService : ISagaDomainService
    {
        private readonly IDictionary<string, Action<ExternalTask>> workers;
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ILog _log;
        private readonly ICreditacaoWithCardCreationDomainService _creditacaoWithCardCreation;
        private readonly ICreditacaoFinalStepDomainService _creditacaoFinalStep;
        private readonly ICreditacaoWithNoCardCreationDomainService _creditacaoWithNoCardCreation;
        private Timer pollingTimer;
        private readonly int _pollingtime;

        public SagaDomainService(ILog log, ICreditacaoFinalStepDomainService creditacaoFinalStep, ICreditacaoWithNoCardCreationDomainService creditacaoWithNoCardCreation, ICreditacaoWithCardCreationDomainService creditacaoWithCardCreation)
        {
            _log = log;
            camundaEngineClient = new CamundaEngineClient(new Uri(BaseConfiguration.ReturnSettingsValue("CamundaConfiguration", "Uri")), null, null);
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
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 1, dueTime);
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });

            registerWorker("department-director", externalTask =>
            {
                Console.WriteLine($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "department-director")));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 2, dueTime);
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });
            registerWorker("scientific-council", externalTask =>
            {
                Console.WriteLine($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                bool result = _creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("CreditacaoISMAI", externalTask); 
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });

            registerWorker("coordenator-jury", externalTask =>
            {
                Console.WriteLine($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "coordenator-jury")));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 0, dueTime, true);
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });

            registerWorker("jury-delibers", externalTask =>
            {
                Console.WriteLine($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                DateTime dueTime = DateTime.Now.AddDays(Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "jury-delibers")));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 2, dueTime, true);
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });

            registerWorker("presidentcouncil-evaluates", externalTask =>
            {
                Console.WriteLine($"Councilium president evaluates task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Councilium president evaluates task is running..{externalTask.Id} -{DateTime.Now}");
                bool result = _creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("CreditacaoISMAI", externalTask);
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });

            registerWorker("final-result", externalTask =>
            {
                Console.WriteLine($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                bool result = _creditacaoFinalStep.FinishProcess("CreditacaoISMAI", externalTask);
                Console.WriteLine($"The process has been completed? {result.ToString()}");
            });

            pollingTimer = new Timer(_ => StartPolling(), null, _pollingtime, Timeout.Infinite);
        }

        private void StartPolling()
        {
            _log.Info($"Time to poll tasks!, workerId is CreditacaoISMAI");
            Console.WriteLine($"Time to poll tasks!, workerId is CreditacaoISMAI - {DateTime.Now}");
            PollTasks();
            pollingTimer.Change(_pollingtime, Timeout.Infinite);
        }
        private void PollTasks()
        {
            try
            {
                var tasks = camundaEngineClient.ExternalTaskService.FetchAndLockTasks("CreditacaoISMAI", 1000000, workers.Keys, 30000, null);
                Parallel.ForEach(
                    tasks,
                    new ParallelOptions { MaxDegreeOfParallelism = 1 },
                    (externalTask) =>
                    {
                        workers[externalTask.TopicName](externalTask);
                    });
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
            }
        }

        private void registerWorker(string topicName, Action<ExternalTask> action)
        {
            workers.Add(topicName, action);
        }
    }
}
