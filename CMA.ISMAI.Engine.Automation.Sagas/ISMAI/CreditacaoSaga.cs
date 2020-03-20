using CamundaClient.Dto;
using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.Interface;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI
{
    public class CreditacaoSaga : ISaga
    {
        private readonly IEngine _engine;
        private readonly ILog _log;
        private readonly IDictionary<string, Action<ExternalTask>> workers;
        private Timer pollingTimer;

        public CreditacaoSaga(IEngine engine, ILog log)
        {
            this._engine = engine;
            this._log = log;
            workers = new Dictionary<string, Action<ExternalTask>>();
        }

        public void RegisterNewWorker()
        {
            _log.Info("A new worker will be added!");

            #region Creation of the workers
            registerWorker("excel-coordenador", externalTask =>
            {
                // Complete task
                _engine.CompleteTask("ISMAI", externalTask.Id, null);
            });
            #endregion

            _log.Info("New workers added!");
        }
        private void registerWorker(string topicName, Action<ExternalTask> action)
        {
            _log.Info($"A new worker task's will be created! - {topicName}");
            workers.Add(topicName, action);
            _log.Info($"A new worker task's added! - {topicName}");
        }

        public void StartTaskPolling()
        {
            pollingTimer = new Timer(_ => pollTasks(), null, 5, Timeout.Infinite);
        }

        private void pollTasks()
        {
            _log.Info($"It's Poll Time in creditacao!!");
            IDictionary<string, Action<ExternalTask>> lockedWorkers = workers;
            var tasks = _engine.FetchAndLockTasks("ISMAI", 10000, lockedWorkers.Keys, 5 * 60 * 1000, null);
            if (tasks.Count > 0) {
                _log.Info($"It's Poll Time in creditacao!!, and we have tasks!");
                Parallel.ForEach(
                    tasks,
                    new ParallelOptions { MaxDegreeOfParallelism = 1 },
                    (externalTask) => {
                        _log.Info($"A new worker task's will be processed! - {externalTask.TopicName}");
                        lockedWorkers[externalTask.TopicName](externalTask);
                    });
            }
            // schedule next run
            _log.Info($"Schedule next run in Creditacao Saga...");
            pollingTimer.Change(5, Timeout.Infinite);
        }

        public IDictionary<string, Action<ExternalTask>> ReturnExternalWorkersTasks()
        {
            return workers;
        }
    }
}
