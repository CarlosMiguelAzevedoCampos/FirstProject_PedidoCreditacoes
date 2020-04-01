using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CMA.ISMAI.Trello.Engine.Automation
{
    public class EngineService : IEngine
    {
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ILog _log;
        private readonly string filePath;

        public EngineService(ILog log)
        {
            this._log = log;
            camundaEngineClient = new CamundaEngineClient(new Uri("http://localhost:8080/engine-rest/engine/default/"), null, null);
            filePath = $"CMA.ISMAI.Trello.Engine.Automation.WorkFlow.creditacaoISMAI.bpmn";
        }
       
        public string StartWorkFlow(string newCardId, string courseName, string studentName, string courseInstitute, bool isCet)
        {
            try
            {
                Dictionary<string, object> parameters = CreateParameters(newCardId, courseName, studentName, courseInstitute, isCet);

                FileParameter file = FileParameter.FromManifestResource(Assembly.GetExecutingAssembly(), filePath);
                _log.Info(string.Format("{0} process workflow will be deployed to the workflow platform", "CreditacaoISMAI"));
                string deployId = camundaEngineClient.RepositoryService.Deploy("CreditacaoISMAI", new List<object> { file });
                if (TheDeployWasDone(deployId, "CreditacaoISMAI"))
                {
                    return camundaEngineClient.BpmnWorkflowService.StartProcessInstance("CreditacaoISMAI", parameters);
                }
                _log.Fatal(string.Format("{0} process workflow deployed to the workflow platform had an Error! No DeployId returned!", "CreditacaoISMAI"));
            }
            catch
            {
                _log.Fatal(string.Format("{0} process workflow deployed to the workflow platform had an Error! Aborting.. {1}", filePath, "CreditacaoISMAI"));
            }
            return string.Empty;
        }

        private static Dictionary<string, object> CreateParameters(string newCardId, string courseName, string studentName, string courseInstitute, bool isCet)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("cardId", newCardId);
            parameters.Add("courseName", courseName);
            parameters.Add("studentName", studentName);
            parameters.Add("courseInstitute", courseInstitute);
            parameters.Add("cet", isCet);
            return parameters;
        }

        private bool TheDeployWasDone(string deployId, string processName)
        {
            if (deployId != string.Empty)
            {
                _log.Info(string.Format("{0} process workflow deployed to the workflow platform", processName));
                return true;
            }
            return false;
        }
    }
}