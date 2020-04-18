using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Base;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class CreditacaoWithCardCreationDomain : ICreditacaoWithCardCreationDomain
    {
        private readonly ICreditacaoDomain _creditacaoService;
        private readonly ITaskProcessing _taskProcessing;
        private readonly ILog _log;

        public CreditacaoWithCardCreationDomain(ICreditacaoDomain creditacaoService, ILog log, ITaskProcessing taskProcessing)
        {
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            _log = log;
        }

        public void CreateCardAndFinishProcess(string processName, ExternalTask externalTask, int boardId, DateTime dueTime, bool IsCetOrOtherCondition)
        {
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - executing..");
            string cardId = _taskProcessing.ReturnValueFromExternalTask(externalTask, "cardId").ToString();
            string courseName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseName").ToString();
            string studentName = _taskProcessing.ReturnValueFromExternalTask(externalTask, "studentName").ToString();
            string courseInstitute = _taskProcessing.ReturnValueFromExternalTask(externalTask, "courseInstitute").ToString();
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - card details obtained from camunda..");

            string newCardId = _creditacaoService.CreditacaoWithNewCardCreation(cardId, courseName, studentName, courseInstitute, dueTime, IsCetOrOtherCondition, boardId);
            if (string.IsNullOrEmpty(newCardId))
                return;
            _taskProcessing.FinishTasks(processName, externalTask.Id, _taskProcessing.returnDictionary(newCardId, courseName, studentName, courseInstitute));
            _log.Info($"{externalTask.Id} - {processName} - {externalTask.TopicName} - completed!");
        }
    }
}
