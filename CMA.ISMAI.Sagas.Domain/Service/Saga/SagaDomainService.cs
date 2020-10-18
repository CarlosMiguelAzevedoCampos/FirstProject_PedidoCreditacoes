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
        private readonly int _taskNumbers;

        public SagaDomainService(ILog log, ICreditacaoFinalStepDomainService creditacaoFinalStep, ICreditacaoWithNoCardCreationDomainService creditacaoWithNoCardCreation, ICreditacaoWithCardCreationDomainService creditacaoWithCardCreation)
        {
            _log = log;
            camundaEngineClient = new CamundaEngineClient(new Uri(BaseConfiguration.ReturnSettingsValue("CamundaConfiguration", "Uri")), null, null);
            _creditacaoWithCardCreation = creditacaoWithCardCreation;
            _creditacaoFinalStep = creditacaoFinalStep;
            _creditacaoWithNoCardCreation = creditacaoWithNoCardCreation;
            workers = new Dictionary<string, Action<ExternalTask>>();
            _pollingtime = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TimerConfiguration", "Time"));
            _taskNumbers = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TimerConfiguration", "Tasks"));
        }
        public void RegistWorkers()
        {
            registerWorker("coursecoordinator", externalTask =>
            {
                _log.Info($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                Console.WriteLine($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                int dueTime = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "coursecoordinator"));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 1, dueTime, "O diretor de departamento deve verificar o processo e remetê-lo ao Conselho Científico");
                Console.WriteLine($"{externalTask.Id} -{DateTime.Now} - The process has been completed? {result.ToString()}");
            });

            registerWorker("departmentdirector", externalTask =>
            {
                Console.WriteLine($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                int dueTime = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "departmentdirector"));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 2, dueTime, "Deve ser decidido, podendo recusar uma parte das creditações caso estas não cumpram a lei ou o establecido no Regulamento.");
                Console.WriteLine($"{externalTask.Id} -{DateTime.Now} - The process has been completed? {result.ToString()}");
            });
            registerWorker("scientific-council", externalTask =>
            {
                Console.WriteLine($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                bool result = _creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("CreditacaoISMAI", externalTask);
                Console.WriteLine($"{externalTask.Id} -{DateTime.Now} - The process has been completed? {result.ToString()}");
            });

            registerWorker("coordenatorjury", externalTask =>
            {
                Console.WriteLine($"Course coordinator Jury task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                int dueTime = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "coordenatorjury"));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 0, dueTime,"O júri delibera sobre o pedido de creditação e o Coordenador de Curso remete o processo para o Conselho Científico.", true);
                Console.WriteLine($"{externalTask.Id} -{DateTime.Now} - The process has been completed? {result.ToString()}");
            });

            registerWorker("jurydelibers", externalTask =>
            {
                Console.WriteLine($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                int dueTime = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("TrelloCardsTime", "jurydelibers"));
                bool result = _creditacaoWithCardCreation.CreateCardAndFinishProcess("CreditacaoISMAI", externalTask, 2, dueTime,"Presidente do conselho científico valida os dados.", true);
                Console.WriteLine($"{externalTask.Id} -{DateTime.Now} - The process has been completed? {result.ToString()}");
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
                Console.WriteLine($"{externalTask.Id} -{DateTime.Now} - The process has been completed? {result.ToString()}");
            });

            pollingTimer = new Timer(_ => StartPolling(), null, _pollingtime, Timeout.Infinite);
        }

        private void StartPolling()
        {
            PollTasks();
            pollingTimer.Change(_pollingtime, Timeout.Infinite);
        }
        private void PollTasks()
        {
            try
            {   
                var tasks = camundaEngineClient.ExternalTaskService.FetchAndLockTasks("CreditacaoISMAI", _taskNumbers, workers.Keys, _pollingtime/2, null);
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
                _log.Fatal(string.Format("{0}, Camunda is starting..?", ex.ToString()));
                Console.WriteLine(string.Format("{0},Camunda is starting..?, let's wait 30 seconds", ex.ToString()));
                Thread.Sleep(30000);
            }
        }

        private void registerWorker(string topicName, Action<ExternalTask> action)
        {
            workers.Add(topicName, action);
        }
    }
}
