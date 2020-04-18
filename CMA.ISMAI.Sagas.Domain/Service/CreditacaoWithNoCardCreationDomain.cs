using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class CreditacaoWithNoCardCreation : ICreditacaoWithNoCardCreationDomain
    {
        private readonly ICreditacaoDomain _creditacaoService;
        private readonly ILog _log;
        private readonly ITaskProcessing _taskProcessing;

        public CreditacaoWithNoCardCreation(ILog log, ICreditacaoDomain creditacaoService, ITaskProcessing taskProcessing)
        {
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            _log = log;
        }

        public void ValidCardStateAndFinishProcess(string processName, ExternalTask externalTask)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = _taskProcessing.ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            string courseName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseName").ToString();
            string studentName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string courseInstitute = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseInstitute").ToString();
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            if (!_creditacaoService.GetCardStatus(cardId))
                return;

            _taskProcessing.FinishTasks(processName, externalTask.Id, _taskProcessing.returnDictionary(cardId, courseName, studentName, courseInstitute));
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
        }
    }
}
