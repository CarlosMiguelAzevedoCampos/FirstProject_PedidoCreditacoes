using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Service.Interface;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class CreditacaoFinalStepDomain : ICreditacaoFinalStepDomain
    {
        private readonly ICreditacaoDomain _creditacaoService;
        private readonly ILog _log;
        private readonly ITaskProcessing _taskProcessing;
        private readonly ISagaNotification _creditacoesNotification;
        public CreditacaoFinalStepDomain(ICreditacaoDomain creditacaoService, ITaskProcessing taskProcessing, ILog log, ISagaNotification creditacoesNotification)
        {
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            _log = log;
            _creditacoesNotification = creditacoesNotification;
        }
        public bool FinishProcess(string processName, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string studentName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string cardId = _taskProcessing.ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            List<string> filesUrl = _creditacaoService.GetCardAttachments(cardId);
            string attachmentsLinks = createStringOfAttachements(filesUrl);
            _creditacoesNotification.SendNotification(new MessageBody(BaseConfiguration.ReturnSettingsValue("EmailSecretaria", "Email"),
                $"Processo de {studentName}, foi terminado. De seguida, seguem os anexos do seu processo. {attachmentsLinks}"));

            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");
            bool finishTaskResult = _taskProcessing.FinishTasks(processName, externalTask.Id);
            string result = string.Format("Task is {0}", finishTaskResult ? "Completed" : "Incompleted.. Waiting for next poll..");
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - {result}!");
            return finishTaskResult;
        }

        private string createStringOfAttachements(List<string> filesUrl)
        {
            string attachmentsLinks = string.Empty;
            foreach (var item in filesUrl)
                attachmentsLinks += item + ",";
            return attachmentsLinks;
        }
    }
}