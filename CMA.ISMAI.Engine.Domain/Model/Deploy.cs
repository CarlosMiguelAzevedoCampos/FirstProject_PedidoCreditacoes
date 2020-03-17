using CMA.ISMAI.Core.Models;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Domain.Model
{
    public class Deploy : Entity
    {
        public Deploy(Guid id, string workFlowName, string processName, Dictionary<string, object> parameters)
        {
            Id = id;
            WorkFlowName = workFlowName;
            ProcessName = processName;
            Parameters = parameters;
        }

        public string WorkFlowName { get; set; }
        public string ProcessName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }

        public bool IsValid(Deploy deploy)
        {
            if (string.IsNullOrEmpty(deploy.ProcessName) || string.IsNullOrEmpty(deploy.WorkFlowName))
                return false;
            return true;
        }
    }
}
