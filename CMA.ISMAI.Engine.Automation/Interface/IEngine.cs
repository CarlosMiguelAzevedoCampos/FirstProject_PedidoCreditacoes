using System.Reflection;

namespace CMA.ISMAI.Automation.Interface
{
    public interface IEngine
    {
        string DeployWorkFlow(string filePath, Assembly assemblyInformation, string processName);
        bool DeleteDeployment(string deploymentId);
    }
}
