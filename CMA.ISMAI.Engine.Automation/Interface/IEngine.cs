using CamundaClient.Dto;
using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Automation.Interface
{
    public interface IEngine
    {
        string StartWorkFlow(string filePath, Assembly assemblyInformation, string processName, Dictionary<string, object> parameters);
    }
}
