using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Domain.Commands;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Engine.Domain.Interface;
using CMA.ISMAI.Logging.Interface;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Domain.CommandHandlers
{
    public class WorkFlowCommandHandler : CommandHandler, IWorkflowCommandHandler
    {
        private readonly IEngine _engine;
        private readonly ILog _log;
        private readonly IWorkflowEventHandler _workflowEventHandler;

        public WorkFlowCommandHandler(IEngine engine, ILog log, IWorkflowEventHandler workflowEventHandler)
        {
            _engine = engine;
            _log = log;
            _workflowEventHandler = workflowEventHandler;
        }
        public Event Handle(StartWorkFlowCommand request)
        {
            Event @event;
            if (!request.IsValid())
            {
                @event = new WorkFlowStartFailedEvent(NotifyValidationErrors(request));
                _workflowEventHandler.Handle(@event as WorkFlowStartFailedEvent);
                return @event;
            }

            string workFlowName = ReturnWorkFlowProcess(request.WorkFlowName);

            if (string.IsNullOrEmpty(workFlowName))
            {
                _log.Fatal(@$"An Deploy order with the workflow name {workFlowName} has made!. But Failed!!!,
                                check the WorkFlowName! TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                @event = new WorkFlowStartFailedEvent(GetWorkFlowNotificationError("WorkFlow Name", "Couldn't find the workflow process name!"));
                _workflowEventHandler.Handle(@event as WorkFlowStartFailedEvent);
                return @event;
            }

            string filePath = $"CMA.ISMAI.Engine.API.WorkFlow.{workFlowName}";
            string getProcessName = obtainProcessName(workFlowName);
            string result = _engine.StartWorkFlow(filePath, request.AssemblyName, getProcessName, request.Parameters);

            return ResultEventReturn(request, workFlowName, result);
        }

        private Event ResultEventReturn(StartWorkFlowCommand request, string workFlowName, string result)
        {
            Event @event;
            if (string.IsNullOrEmpty(result))
            {
                _log.Fatal($"An Deploy order with the workflow name {workFlowName} has made!. But Failed!!! TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                @event = new WorkFlowStartFailedEvent(GetWorkFlowNotificationError("WorkFlow Start Failed!", "Couldn't start the workflow!"));
                _workflowEventHandler.Handle(@event as WorkFlowStartFailedEvent);
                return @event;
            }
            _log.Info($"An Deploy order with the workflow name {workFlowName} has made!. " +
                $"Was deployed? {result.ToString()}");
            @event = new WorkFlowStartCompletedEvent(result, request.WorkFlowName);
            _workflowEventHandler.Handle(@event as WorkFlowStartCompletedEvent);
            return @event;
        }

        private List<DomainNotification> GetWorkFlowNotificationError(string key, string value)
        {
            List<DomainNotification> domainNotification = new List<DomainNotification>();
            domainNotification.Add(new DomainNotification(key, value));
            return domainNotification;
        }

        private string obtainProcessName(string workFlowName)
        {
            switch (workFlowName)
            {
                case "creditacaoISMAI.bpmn":
                    return "CreditacaoISMAI";
                default:
                    return string.Empty;
            }
        }

        private string ReturnWorkFlowProcess(string workFlowName)
        {
            switch (workFlowName)
            {
                case "ISMAI":
                    return "creditacaoISMAI.bpmn";
                default:
                    return string.Empty;
            }
        }
    }
}
