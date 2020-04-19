using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;

namespace CMA.ISMAI.Sagas.Domain.Service.Creditacao
{
    public class CreditacaoWithNoCardCreationService : ICreditacaoWithNoCardCreationDomainService
    {
        private readonly ICreditacaoDomainService _creditacaoService;
        private readonly ILog _log;
        private readonly ITaskProcessingDomainService _taskProcessing;

        public CreditacaoWithNoCardCreationService(ILog log, ICreditacaoDomainService creditacaoService, ITaskProcessingDomainService taskProcessing)
        {
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            _log = log;
        }

        public bool ValidCardStateAndFinishProcess(string processName, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            if (ItsSummerBreakTime(DateTime.Now.Month))
                return false;
            string cardId = _taskProcessing.ReturnCardIdFromExternalTask(externalTask);
            string courseName = _taskProcessing.ReturnCourseNameFromExternalTask(externalTask);
            string studentName = _taskProcessing.ReturnStudentNameFromExternalTask(externalTask);
            string courseInstitute = _taskProcessing.ReturnCourseInstitueFromExternalTask(externalTask);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            if (!_creditacaoService.GetCardStatus(cardId))
                return false;

            return ReturnFinishTaskResult(processName, externalTask, cardId, courseName, studentName, courseInstitute);
        }

        private bool ItsSummerBreakTime(int month)
        {
            return _creditacaoService.IsSummerBreakTime(month);
        }

        private bool ReturnFinishTaskResult(string processName, ExternalTask externalTask, string cardId, string courseName, string studentName, string courseInstitute)
        {
            if (!_taskProcessing.FinishTasks(processName, externalTask.Id, _taskProcessing.ReturnDictionaryForTheProcess(cardId, courseName, studentName, courseInstitute)))
            {
                _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - Task isn't finished.., waiting for the next pool");
                return false;
            }
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
            return true;
        }
    }
}
