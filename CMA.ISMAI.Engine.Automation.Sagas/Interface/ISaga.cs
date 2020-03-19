using CamundaClient.Dto;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Automation.Sagas.Interface
{
    public interface ISaga
    {
        void RegisterNewWorker();
        void StartTaskPolling();
        IDictionary<string, Action<ExternalTask>> ReturnExternalWorkersTasks();
    }
}
