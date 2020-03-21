using CamundaClient;
using CamundaClient.Dto;
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
            registerWorker("book-hotel", externalTask =>
            {
                Console.WriteLine($"Book hotel now...{externalTask.Id} -{DateTime.Now}"); // e.g. by calling a REST endpoint

                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("cardId", Guid.NewGuid().ToString());
                camundaEngineClient.ExternalTaskService.Complete(processName, externalTask.Id, keyValuePairs);
            });
            registerWorker("book-car", externalTask =>
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("cardId", Guid.NewGuid().ToString());
                string cardId = externalTask.Variables.GetValueOrDefault("cardId").Value.ToString();
                Console.WriteLine($"Book car now... {externalTask.Id} - {DateTime.Now} --- {cardId}"); // e.g. by calling a REST endpoint
                camundaEngineClient.ExternalTaskService.Complete(processName, externalTask.Id, keyValuePairs);
            });
            registerWorker("book-flight", externalTask =>
            {
                string cardId = externalTask.Variables.GetValueOrDefault("cardId").Value.ToString();
                Console.WriteLine($"Book flight now...{externalTask.Id} -{DateTime.Now} --- {cardId}"); // e.g. by calling a REST endpoint
                camundaEngineClient.ExternalTaskService.Complete(processName, externalTask.Id, null);
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
