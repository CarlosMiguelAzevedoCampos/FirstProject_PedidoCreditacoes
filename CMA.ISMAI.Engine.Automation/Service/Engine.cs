using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Automation.Service
{
    public class Engine : IEngine
    {
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ILog _log;

        public Engine(ILog log)
        {
            this._log = log;
            camundaEngineClient = new CamundaEngineClient(new Uri("http://localhost:8080/engine-rest/engine/default/"), null, null);
        }

        public bool DeleteDeployment(string deploymentId)
        {
            try
            {
                _log.Info(string.Format("{0} workflow deployed will be deleted from the workflow platform", deploymentId));
                camundaEngineClient.RepositoryService.DeleteDeployment(deploymentId);
            }
            catch (Exception ex)
            {
                _log.Fatal(string.Format("An error happend deleting the deployment {0}.. Aborting ", deploymentId, ex));
                return false;
            }
            return true;
        }
        
        public string DeployWorkFlow(string filePath, Assembly assemblyInformation, string processName)
        {
            try
            {
                FileParameter file = FileParameter.FromManifestResource(assemblyInformation, filePath);
                _log.Info(string.Format("{0} process workflow will be deployed to the workflow platform", processName));
                string deployId = camundaEngineClient.RepositoryService.Deploy(processName, new List<object> { file });
                _log.Info(string.Format("{0} process workflow deployed to the workflow platform", processName));
                return deployId;
            }
            catch (Exception ex)
            {
                _log.Fatal(string.Format("{0} process workflow deployed to the workflow platform had an Error! Aborting.. {1}", filePath, ex));
                return string.Empty;
            }
        }
    }
}
