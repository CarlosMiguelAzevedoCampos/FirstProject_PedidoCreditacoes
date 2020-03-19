using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void CompleteTask(string workerId, string id)
        {
            try
            {
                _log.Fatal($"Task with the workerId {workerId}, will be completed!");
                camundaEngineClient.ExternalTaskService.Complete(workerId, id);
                _log.Fatal($"Task with the workerId {workerId}, completed!");
            }
            catch
            {
                _log.Fatal($"Task with the workerId {workerId}, couldn't be completed!");
            }
        }

        public List<ExternalTask> FetchAndLockTasks(string workerId, int maxTasks, IEnumerable<string> topicNames, long lockDurationInMilliseconds, IEnumerable<string> variablesToFetch = null)
        {
            try
            {
                _log.Info($"Fetching tasks for saga worker {workerId}");
                return camundaEngineClient.ExternalTaskService.FetchAndLockTasks(workerId, maxTasks, topicNames, lockDurationInMilliseconds, variablesToFetch).ToList();
            }
            catch
            {
                _log.Fatal($"Ups.., fetching and lock tasks failed!!!, for the worker {workerId}");
            }
            return new List<ExternalTask>();
        }

        public string StartWorkFlow(string filePath, Assembly assemblyInformation, string processName, Dictionary<string, object> parameters)
        {
            try
            {
                FileParameter file = FileParameter.FromManifestResource(assemblyInformation, filePath);
                _log.Info(string.Format("{0} process workflow will be deployed to the workflow platform", processName));
                string deployId = camundaEngineClient.RepositoryService.Deploy(processName, new List<object> { file });
                if (TheDeployWasDone(deployId, processName))
                {
                    return StartProcess("FlowingTripBookingSaga", parameters);
                }
                _log.Fatal(string.Format("{0} process workflow deployed to the workflow platform had an Error! No DeployId returned!", processName));
            }
            catch(Exception ex)
            {
                _log.Fatal(string.Format("{0} process workflow deployed to the workflow platform had an Error! Aborting.. {1}", filePath, processName));
            }
            return string.Empty;
        }

        private string StartProcess(string processName, Dictionary<string, object> parameters)
        {
            return camundaEngineClient.BpmnWorkflowService.StartProcessInstance(processName, parameters);
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
