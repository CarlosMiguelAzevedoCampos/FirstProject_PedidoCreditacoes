using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class CreditacaoWithCardCreationDomainService : ICreditacaoWithCardCreationDomainService
    {
        private readonly ICreditacaoDomainService _creditacaoService;
        private readonly ITaskProcessingDomainService _taskProcessing;
        private readonly ILog _log;

        public CreditacaoWithCardCreationDomainService(ICreditacaoDomainService creditacaoService, ILog log, ITaskProcessingDomainService taskProcessing)
        {
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            _log = log;
        }

        public bool CreateCardAndFinishProcess(string processName, ExternalTask externalTask, int boardId, DateTime dueTime, bool IsCetOrOtherCondition)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = _taskProcessing.ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            string courseName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseName").ToString();
            string studentName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string courseInstitute = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseInstitute").ToString();
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            string newCardId = _creditacaoService.CreateNewCard(cardId, courseName, studentName, courseInstitute, dueTime, IsCetOrOtherCondition, boardId);

            if (string.IsNullOrEmpty(newCardId))
                return false;

            return ReturnFinishTaskResult(processName, externalTask, courseName, studentName, courseInstitute, newCardId);
        }

        private bool ReturnFinishTaskResult(string processName, ExternalTask externalTask, string courseName, string studentName, string courseInstitute, string newCardId)
        {
            if (!_taskProcessing.FinishTasks(processName, externalTask.Id, _taskProcessing.ReturnDictionaryForTheProcess(newCardId, courseName, studentName, courseInstitute)))
            {
                _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - Task isn't finished.., waiting for the next pool");
                TryToDeleteTheNewCardCreated(newCardId);
                return false;
            }
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
            return true;
        }

        private void TryToDeleteTheNewCardCreated(string newCardId)
        {
            bool result = _creditacaoService.DeleteCard(newCardId);
           _log.Info($"Card has been deleted from Trello? {result.ToString()}.. Process failed to finish");
        }
    }
}