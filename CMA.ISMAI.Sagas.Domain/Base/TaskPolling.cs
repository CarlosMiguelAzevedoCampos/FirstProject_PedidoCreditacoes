using CamundaClient;
using CamundaClient.Dto;
using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas.Domain.Base
{
    public abstract class TaskPolling
    {
        private readonly CamundaEngineClient camundaEngineClient;
        private readonly ILog _log;

        public TaskPolling(ILog log)
        {
            _log = log;
            camundaEngineClient = new CamundaEngineClient(new Uri(BaseConfiguration.ReturnSettingsValue("CamundaConfiguration", "Uri")), null, null);
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
    }
}
