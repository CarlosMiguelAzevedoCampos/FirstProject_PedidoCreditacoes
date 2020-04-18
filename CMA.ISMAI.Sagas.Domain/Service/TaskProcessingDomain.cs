using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Service
{
    public class TaskProcessingDomain : ITaskProcessing
    {
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ICreditacaoDomain _creditacaoService;
        private readonly ITaskProcessing _taskProcessing;
        private readonly ILog _log;

        public TaskProcessingDomain(ILog log, ICreditacaoDomain creditacaoService, ITaskProcessing taskProcessing)
        {
            _log = log;
            _creditacaoService = creditacaoService;
            _taskProcessing = taskProcessing;
            camundaEngineClient = new CamundaEngineClient(new Uri(BaseConfiguration.ReturnSettingsValue("CamundaConfiguration", "Uri")), null, null);
        }

        public bool FinishTasks(string processName, string Id, Dictionary<string, object> taskValues = null)
        {
            try
            {
                camundaEngineClient.ExternalTaskService.Complete(processName, Id, taskValues);
                return true;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return false;
            }
        }

        public object ReturnValueFromExternalTask(ExternalTask externalTask, string key)
        {
            try
            {
                return externalTask.Variables.GetValueOrDefault(key).Value;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return string.Empty;
            }
        }

        public bool CheckIfItsSummerBreak(DateTime dateTime)
        {
            return _creditacaoService.IsSummerBreakTime(dateTime.Month);
        }

        public Dictionary<string, object> returnDictionary(string newCardId, string courseName, string studentName, string courseInstitute)
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
