using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas.Services.Base
{
    public abstract class Saga
    {
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ILog _log;

        public Saga(ILog log)
        {
            _log = log;
            camundaEngineClient = new CamundaEngineClient(new Uri("http://localhost:8080/engine-rest/engine/default/"), null, null);
        }

        protected void PollTasks(string workerId, IDictionary<string, Action<ExternalTask>> workers)
        {
            try
            {
                var tasks = camundaEngineClient.ExternalTaskService.FetchAndLockTasks(workerId, 1000000, workers.Keys, 30000, null);
                Parallel.ForEach(
                    tasks,
                    new ParallelOptions { MaxDegreeOfParallelism = 1 },
                    (externalTask) =>
                    {
                        workers[externalTask.TopicName](externalTask);
                    });
            }
            catch(Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
            }
        }

        protected bool FinishTasks(string processName, string Id, Dictionary<string,object> taskValues = null)
        {
            try
            {
                camundaEngineClient.ExternalTaskService.Complete(processName, Id, taskValues);
                return true;
            }
            catch(Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return false;
            }
        }

        protected object ReturnValueFromExternalTask(ExternalTask externalTask, string key)
        {
            try
            {
                return externalTask.Variables.GetValueOrDefault(key).Value;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException.ToString());
                return string.Empty;
            }
        }
    }
}
