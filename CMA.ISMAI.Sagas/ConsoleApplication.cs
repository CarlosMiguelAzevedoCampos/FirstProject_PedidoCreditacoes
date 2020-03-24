﻿using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas
{
    public class ConsoleApplication
    {
        private CamundaEngineClient camundaEngineClient;
        private IDictionary<string, Action<ExternalTask>> workers;
        private Timer pollingTimer;
        private ILog _log;
        private ICreditacoesService _creditacoesService;


        public ConsoleApplication(ILog log, ICreditacoesService creditacoesService)
        {
            _log = log;
            _creditacoesService = creditacoesService;
            camundaEngineClient = new CamundaEngineClient(new Uri("http://localhost:8080/engine-rest/engine/default/"), null, null);
            workers = new Dictionary<string, Action<ExternalTask>>();
        }
        internal void Run()
        {
            _log.Info("Sagas started now!");
            Console.WriteLine("Sagas started...");
            _log.Info("Sagas startíng for CreditacaoISMAI!");
            new Thread(() => RegistCreditacoesWorkers("CreditacaoISMAI")).Start();
        }

        private void registerWorker(string topicName, Action<ExternalTask> action)
        {
            workers.Add(topicName, action);
        }

        private void RegistCreditacoesWorkers(string processName)
        {
            registerWorker("course-coordinator", externalTask =>
            {
                _log.Info($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                Console.WriteLine($"Course coordinator task is running..{externalTask.Id} -{DateTime.Now}");
                creditacoesSaga(processName, externalTask, 1, DateTime.Now.AddDays(2));
            });

            registerWorker("department-director", externalTask =>
            {
                Console.WriteLine($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                creditacoesSaga(processName, externalTask, 1, DateTime.Now.AddDays(2));
            });
            registerWorker("scientific-council", externalTask =>
            {
                Console.WriteLine($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                creditacoesSaga(processName, externalTask, 1, DateTime.Now.AddDays(2));

            });

            registerWorker("final-result", externalTask =>
            {
                Console.WriteLine($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                creditacoesSagaFinal(processName, externalTask);
            });

            pollingTimer = new Timer(_ => PollTasks("CreditacaoISMAI"), null, 1, Timeout.Infinite);
        }


        private void PollTasks(string workerId)
        {
            _log.Info($"Time to poll tasks!, workerId is {workerId}");
            Console.WriteLine($"Time to poll tasks!, workerId is {workerId} - {DateTime.Now}");

            var tasks = camundaEngineClient.ExternalTaskService.FetchAndLockTasks(workerId, 1000000, workers.Keys, 2 * 60 * 1000, null);
            Parallel.ForEach(
                tasks,
                new ParallelOptions { MaxDegreeOfParallelism = 1 },
                (externalTask) =>
                {
                    workers[externalTask.TopicName](externalTask);
                });

            pollingTimer.Change(1, Timeout.Infinite);
        }
        private void creditacoesSaga(string processName, ExternalTask externalTask, int boardId, DateTime dueTime)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = getCardId(externalTask);
            string courseName = getCourseName(externalTask);
            string studentName = getStudentName(externalTask);
            string courseInstitute = getInstituteName(externalTask);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            if (!getCardStatus(cardId))
                return;

            string newCardId = _creditacoesService.PostNewCard(new CardDto($"{courseInstitute} - {courseName} - {studentName}",
                dueTime, boardId, $"{courseInstitute} - {courseName} - {studentName} - A new card has been created. When this task is done, please check it has done"));
            if (string.IsNullOrEmpty(newCardId))
                return;
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - creating variables for the next proccess..");
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("cardId", newCardId);
            keyValuePairs.Add("courseName", courseName);
            keyValuePairs.Add("studentName", studentName);
            keyValuePairs.Add("courseInstitute", courseInstitute);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");

            camundaEngineClient.ExternalTaskService.Complete(processName, externalTask.Id, keyValuePairs);
        }

        private void creditacoesSagaFinal(string processName, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = getCardId(externalTask);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");
            if (!getCardStatus(cardId))
                return;
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
            camundaEngineClient.ExternalTaskService.Complete(processName, externalTask.Id);
        }

        private string getInstituteName(ExternalTask externalTask)
        {
            return externalTask.Variables.GetValueOrDefault("courseInstitute").Value.ToString();
        }

        private string getStudentName(ExternalTask externalTask)
        {
            return externalTask.Variables.GetValueOrDefault("studentName").Value.ToString();
        }

        private string getCourseName(ExternalTask externalTask)
        {
            return externalTask.Variables.GetValueOrDefault("courseName").Value.ToString();
        }

        private string getCardId(ExternalTask externalTask)
        {
            return externalTask.Variables.GetValueOrDefault("cardId").Value.ToString();
        }

        private bool getCardStatus(string cardId)
        {
            bool getCardStatus = _creditacoesService.GetCardState(cardId);
            return getCardStatus;
        }
    }
}