using CMA.ISMAI.Engine.Domain.Model;
using System.Reflection;

namespace CMA.ISMAI.Automation.DomainInterface
{
    public interface IEngineService
    {
        string DeployWorkFlow(Deploy deployPath, Assembly assembly);
        bool DeleteDeployment(string deploymentId);

    }
}
