using CamundaClient.Dto;
using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Automation.Interface
{
    public interface IEngine
    {
        string StartWorkFlow(string filePath, Assembly assemblyInformation, string processName, Dictionary<string, object> parameters);
        List<ExternalTask> FetchAndLockTasks(string workerId, int maxTasks, IEnumerable<string> topicNames, long lockDurationInMilliseconds, IEnumerable<string> variablesToFetch = null);
        void CompleteTask(string workerId, string id, Dictionary<string, object> parameters);
    }
}
