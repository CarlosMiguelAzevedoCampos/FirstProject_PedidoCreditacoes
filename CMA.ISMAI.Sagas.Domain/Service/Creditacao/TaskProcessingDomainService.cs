using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Service.Creditacao
{
    public class TaskProcessingDomainService : ITaskProcessingDomainService
    {
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ICreditacaoDomainService _creditacaoService;
        private readonly ILog _log;

        public TaskProcessingDomainService(ILog log, ICreditacaoDomainService creditacaoService)
        {
            _log = log;
            _creditacaoService = creditacaoService;
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

        public bool CheckIfItsSummerBreak(DateTime dateTime)
        {
            return _creditacaoService.IsSummerBreakTime(dateTime.Month);
        }

        public Dictionary<string, object> ReturnDictionaryForTheProcess(string newCardId, string courseName, string studentName, string courseInstitute)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("cardId", newCardId);
            keyValuePairs.Add("courseName", courseName);
            keyValuePairs.Add("studentName", studentName);
            keyValuePairs.Add("courseInstitute", courseInstitute);
            return keyValuePairs;
        }

        public string ReturnCardIdFromExternalTask(ExternalTask externalTask)
        {
            try
            {
                return externalTask.Variables.GetValueOrDefault("cardId").Value.ToString();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return string.Empty;
            }
        }

        public string ReturnCourseNameFromExternalTask(ExternalTask externalTask)
        {
            try
            {
                return externalTask.Variables.GetValueOrDefault("courseName").Value.ToString();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return string.Empty;
            }
        }

        public string ReturnStudentNameFromExternalTask(ExternalTask externalTask)
        {
            try
            {
                return externalTask.Variables.GetValueOrDefault("studentName").Value.ToString();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return string.Empty;
            }
        }

        public string ReturnCourseInstitueFromExternalTask(ExternalTask externalTask)
        {
            try
            {
                return externalTask.Variables.GetValueOrDefault("courseInstitute").Value.ToString();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return string.Empty;
            }
        }
    }
}
