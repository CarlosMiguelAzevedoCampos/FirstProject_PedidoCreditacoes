using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;

namespace CMA.ISMAI.Sagas.Domain.Service.Creditacao
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

        public bool CreateCardAndFinishProcess(string processName, ExternalTask externalTask, int boardId, int dueTime,string description, bool IsCetOrOtherCondition)
        {
            DateTime createDueTime = _creditacaoService.AddWorkingDays(DateTime.Now, dueTime);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            if (ItsSummerBreakTime(DateTime.Now.Month) || ItsSummerBreakTime(createDueTime.Month))
                return false;
            string cardId = _taskProcessing.ReturnCardIdFromExternalTask(externalTask);
            string courseName = _taskProcessing.ReturnCourseNameFromExternalTask(externalTask);
            string studentName = _taskProcessing.ReturnStudentNameFromExternalTask(externalTask);
            string courseInstitute = _taskProcessing.ReturnCourseInstitueFromExternalTask(externalTask);
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            if (!_creditacaoService.GetCardStatus(cardId))
                return false;
            
            string newCardId = _creditacaoService.CreateNewCard(cardId, description, courseName, studentName, courseInstitute, createDueTime, IsCetOrOtherCondition, boardId);

            if (string.IsNullOrEmpty(newCardId))
                return false;

            return ReturnFinishTaskResult(processName, externalTask, courseName, studentName, courseInstitute, newCardId);
        }

        private bool ItsSummerBreakTime(int month)
        {
            return _creditacaoService.IsSummerBreakTime(month);
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