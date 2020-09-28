using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Service.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Service.Creditacao
{
    public class CreditacaoFinalStepDomainService : ICreditacaoFinalStepDomainService
    {
        private readonly ICreditacaoDomainService _creditacaoService;
        private readonly ILog _log;
        private readonly ITaskProcessingDomainService _taskProcessing;
        private readonly ISagaNotification _creditacoesNotification;
        public CreditacaoFinalStepDomainService(ICreditacaoDomainService creditacaoService, ITaskProcessingDomainService taskProcessing, ILog log, ISagaNotification creditacoesNotification)
        {
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            _log = log;
            _creditacoesNotification = creditacoesNotification;
        }
        public bool FinishProcess(string processName, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            if (ItsSummerBreakTime())
                return false;
            if (_taskProcessing.FinishTasks(processName, externalTask.Id))
            {
                string cardId = _taskProcessing.ReturnCardIdFromExternalTask(externalTask);
                string studentName = _taskProcessing.ReturnStudentNameFromExternalTask(externalTask);
                List<string> filesUrl = _creditacaoService.GetCardAttachments(cardId);
                string attachmentsLinks = createStringOfAttachements(filesUrl);
                _creditacoesNotification.SendNotification(BaseConfiguration.ReturnSettingsValue("EmailSecretaria", "Email"),
                    $"Final do Processo.  <br/> <br/> Processo de {studentName}, foi terminado. <br/> <br/> De seguida, seguem os anexos do seu processo. <br/><br/> {attachmentsLinks}");
                _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - Task is completed");
                return true;
            }
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - Task isn't finished.., waiting for the next pool");
            return false;
        }

        private bool ItsSummerBreakTime()
        {
            return _creditacaoService.IsSummerBreakTime(DateTime.Now.Month);
        }

        private string createStringOfAttachements(List<string> filesUrl)
        {
            string attachmentsLinks = string.Empty;
            foreach (var item in filesUrl)
                attachmentsLinks += item + " <br/> <br/> ";
            return attachmentsLinks;
        }
    }
}