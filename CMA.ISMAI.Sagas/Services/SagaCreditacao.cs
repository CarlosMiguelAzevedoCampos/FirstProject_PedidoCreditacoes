using CamundaClient.Dto;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using CMA.ISMAI.Sagas.Services.Base;
using CMA.ISMAI.Sagas.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CMA.ISMAI.Sagas.Creditacoes
{
    public class SagaCreditacao : Saga, ISagaCreditacoesWorker
    {
        private readonly IDictionary<string, Action<ExternalTask>> workers;
        private readonly ILog _log;
        private readonly ICreditacoesNotification _creditacoesNotification;
        private readonly ICreditacaoService _creditacaoService;
        private Timer pollingTimer;
        private readonly int _pollingtime;

        public SagaCreditacao(ILog log, ICreditacaoService creditacaoService, ICreditacoesNotification creditacoesNotification) : base(log)
        {
            _log = log;
            _creditacoesNotification = creditacoesNotification;
            _creditacaoService = creditacaoService;
            workers = new Dictionary<string, Action<ExternalTask>>();
            _pollingtime = 30000;
        }
        public void RegistWorkers()
        {
            registerWorker("course-coordinator", externalTask =>
            {
                _log.Info($"Course coordinator task non-cet is running..{externalTask.Id} -{DateTime.Now}");
                Console.WriteLine($"Course coordinator  non-cet task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoWithCardCreation("CreditacaoISMAI", externalTask, 1, DateTime.Now.AddDays(20));
            });

            registerWorker("department-director", externalTask =>
            {
                Console.WriteLine($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Department director task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoWithCardCreation("CreditacaoISMAI", externalTask, 2, DateTime.Now.AddDays(5));
            });
            registerWorker("scientific-council", externalTask =>
            {
                Console.WriteLine($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Scientific council task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoWithNoCardCreation("CreditacaoISMAI", externalTask);
            });

            registerWorker("coordenator-jury", externalTask =>
            {
                Console.WriteLine($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Course coordinator CET task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoWithCardCreation("CreditacaoISMAI", externalTask, 0, DateTime.Now.AddDays(2), true);
            });

            registerWorker("jury-delibers", externalTask =>
            {
                Console.WriteLine($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Jury delibers task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoWithCardCreation("CreditacaoISMAI", externalTask, 2, DateTime.Now.AddDays(2), true);
            });

            registerWorker("presidentcouncil-evaluates", externalTask =>
            {
                Console.WriteLine($"Councilium president evaluates task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Councilium president evaluates task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoWithNoCardCreation("CreditacaoISMAI", externalTask);
            });

            registerWorker("final-result", externalTask =>
            {
                Console.WriteLine($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                _log.Info($"Final result task is running..{externalTask.Id} -{DateTime.Now}");
                creditacaoFinalStep("CreditacaoISMAI", 2, externalTask);
            });

            pollingTimer = new Timer(_ => StartPollCreditacoesTask("CreditacaoISMAI"), null, _pollingtime, Timeout.Infinite);
        }

        private void StartPollCreditacoesTask(string workerId)
        {
            _log.Info($"Time to poll tasks!, workerId is {workerId}");
            Console.WriteLine($"Time to poll tasks!, workerId is {workerId} - {DateTime.Now}");
            PollTasks("CreditacaoISMAI", workers);
            pollingTimer.Change(_pollingtime, Timeout.Infinite);
        }

        private void registerWorker(string topicName, Action<ExternalTask> action)
        {
            workers.Add(topicName, action);
        }

        private void creditacaoWithCardCreation(string processName, ExternalTask externalTask, int boardId, DateTime dueTime, bool isCet = false)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            string courseName = ReturnValueFromExternalTask(externalTask, "courseName").ToString();
            string studentName = ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string courseInstitute = ReturnValueFromExternalTask(externalTask, "courseInstitute").ToString();
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            string newCardId = _creditacaoService.CreditacaoWithNewCardCreation(cardId, courseName, studentName, courseInstitute, dueTime, isCet, boardId);
            if(string.IsNullOrEmpty(newCardId))
                return;
            FinishTasks(processName, externalTask.Id, returnDictionary(newCardId, courseName, studentName, courseInstitute));
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
        }


        private void creditacaoWithNoCardCreation(string processName, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            string courseName = ReturnValueFromExternalTask(externalTask, "courseName").ToString();
            string studentName = ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string courseInstitute = ReturnValueFromExternalTask(externalTask, "courseInstitute").ToString();
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");
           
            if (!_creditacaoService.GetCardStatus(cardId))
                return;
            
            FinishTasks(processName, externalTask.Id, returnDictionary(cardId, courseName, studentName, courseInstitute));
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
        }
        private void creditacaoFinalStep(string processName, int boardId, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");

            string cardId = ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            List<string> filesUrl = _creditacaoService.GetCardAttachments(cardId);
            
            _creditacoesNotification.SendNotification(new MessageBody("", "Olá"));
            
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");
            FinishTasks(processName, externalTask.Id);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
        }

        private Dictionary<string, object> returnDictionary(string newCardId, string courseName, string studentName, string courseInstitute)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("cardId", newCardId);
            keyValuePairs.Add("courseName", courseName);
            keyValuePairs.Add("studentName", studentName);
            keyValuePairs.Add("courseInstitute", courseInstitute);
            return keyValuePairs;
        }
    }
}
