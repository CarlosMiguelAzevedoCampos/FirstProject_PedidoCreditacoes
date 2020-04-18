using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;

namespace CMA.ISMAI.Sagas.Domain.Service
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
            string cardId = _taskProcessing.ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            string courseName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseName").ToString();
            string studentName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string courseInstitute = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseInstitute").ToString();
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            if (!_creditacaoService.GetCardStatus(cardId))
                return false;

           return ReturnFinishTaskResult(processName, externalTask, cardId, courseName, studentName, courseInstitute);
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
