using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Domain.Commands;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Logging.Interface;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Domain.CommandHandlers
{
    public class WorkFlowCommandHandler : CommandHandler,
        IRequestHandler<StartWorkFlowCommand, bool>
    {
        private readonly IMediatorHandler Bus;
        private readonly IEngine _engine;
        private readonly ILog _log;

        public WorkFlowCommandHandler(IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications, IEngine engine, ILog log) : base(bus, notifications)
        {
            Bus = bus;
            _engine = engine;
            _log = log;
        }
        public Task<bool> Handle(StartWorkFlowCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            string workFlowName = ReturnWorkFlowProcess(request.WorkFlowName);
            if (string.IsNullOrEmpty(workFlowName))
            {
                _log.Fatal(@$"An Deploy order with the process name {request.ProcessName} has made!. But Failed!!!,
                                check the WorkFlowName! TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                Bus.RaiseEvent(new DomainNotification(request.MessageType, "The deploy process failed! Invalid WorkFlowName!"));
                return Task.FromResult(false);
            }

            string filePath = $"CMA.ISMAI.Engine.API.WorkFlow.{workFlowName}";
            string result = _engine.StartWorkFlow(filePath, request.AssemblyName, string.Format("{0}-{1}", request.ProcessName, request.WorkFlowName), true);

            if (string.IsNullOrEmpty(result))
            {
                _log.Fatal($"An Deploy order with the process name {request.ProcessName} has made!. But Failed!!! TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                Bus.RaiseEvent(new DomainNotification(request.MessageType, "The deploy process failed! Empty deploymentId!"));
                return Task.FromResult(false);
            }

            _log.Info($"An Deploy order with the process name {request.ProcessName} has made!. " +
                $"Was deployed? {result.ToString()}");

            Bus.RaiseEvent(new WorkFlowStartCompletedEvent(request.Id, request.WorkFlowName, request.ProcessName, true));

            return Task.FromResult(true);
        }
        
        private string ReturnWorkFlowProcess(string workFlowName)
        {
            switch (workFlowName)
            {
                case "ISMAI":
                    return "ismai.creditacao.bpmn";
                default:
                    return string.Empty;
            }
        }
    }
}
