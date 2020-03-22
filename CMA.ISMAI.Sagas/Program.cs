using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas
{
    class Program
    {
        private static CamundaEngineClient camundaEngineClient;
        private static IDictionary<string, Action<ExternalTask>> workers;
        private static Timer pollingTimer;
        private static ILog _log;
        private static ICreditacoesService _creditacoesService;

        static void Main(string[] args)
        {
            Console.WriteLine("Sagas started...");
            camundaEngineClient = new CamundaEngineClient(new Uri("http://localhost:8080/engine-rest/engine/default/"), null, null);
            workers = new Dictionary<string, Action<ExternalTask>>();

            new Thread(() => RegistCreditacoesWorkers("FlowingTripBookingSaga")).Start();

            Console.ReadKey();
        }

        private static void registerWorker(string topicName, Action<ExternalTask> action)
        {
            workers.Add(topicName, action);
        }

        private static void RegistCreditacoesWorkers(string processName)
        {
            registerWorker("course-coordinator-excel", externalTask =>
            {
                Console.WriteLine($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                string cardId = externalTask.Variables.GetValueOrDefault("cardId").Value.ToString();
                if (string.IsNullOrEmpty(cardId))
                {
                    Console.WriteLine($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                    _log.Info($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}, but cardId is null or empty!!");
                }
                string newCardId = _creditacoesService.CoordenatorExcelAction(cardId, string.Empty);
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("cardId", newCardId);
                camundaEngineClient.ExternalTaskService.Complete(processName, externalTask.Id, keyValuePairs);
            });

            pollingTimer = new Timer(_ => PollTasks("FlowingTripBookingSaga"), null, 1, Timeout.Infinite);
        }

        private static void PollTasks(string workerId)
        {
            var tasks = camundaEngineClient.ExternalTaskService.FetchAndLockTasks(workerId, 1000000, workers.Keys, 5 * 60 * 1000, null);
            Parallel.ForEach(
                tasks,
                new ParallelOptions { MaxDegreeOfParallelism = 1 },
                (externalTask) =>
                {
                    workers[externalTask.TopicName](externalTask);
                });

            pollingTimer.Change(1, Timeout.Infinite);
        }
    }
}
