using CMA.ISMAI.Automation.DomainInterface;
using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Engine.Domain.Model;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Reflection;

namespace CMA.ISMAI.Automation.Service
{
    public class EngineService : IEngineService
    {
        private readonly IEngine _engine;
        private readonly ILog _log;

        public EngineService(IEngine engine, ILog log)
        {
            this._engine = engine;
            this._log = log;
        }

        public bool DeleteDeployment(string deploymentId)
        {
            if (CheckIfDeploymentIdIsNull(deploymentId))
                return false;
            return _engine.DeleteDeployment(deploymentId);
        }

        private bool CheckIfDeploymentIdIsNull(string deploymentId)
        {
            if (string.IsNullOrEmpty(deploymentId))
            {
                return true;
            }
            return false;
        }

        public string StartWorkFlow(Deploy deploy, Assembly assembly)
        {
            if (!deploy.IsValid(deploy))
                return string.Empty;

            string workFlowName = ReturnWorkFlowProcess(deploy.WorkFlowName);
            string filePath = $"CMA.ISMAI.Engine.API.WorkFlow.{workFlowName}";
            string result = _engine.StartWorkFlow(filePath, assembly, string.Format("{0}-{1}", deploy.ProcessName, deploy.WorkFlowName), deploy.IsCet);

            _log.Info($"An Deploy order with the process name {deploy.ProcessName} has made!. " +
                $"Was deployed? {result.ToString()}");

            return result;
        }

        private string ReturnWorkFlowProcess(string workFlowName)
        {
            switch (workFlowName)
            {
                case "ISMAI":
                    return "ismai.creditacao.bpmn";
                default:
                    return string.Empty;
            }
        }
    }
}
