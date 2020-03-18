using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Domain.CommandHandlers;
using CMA.ISMAI.Engine.Domain.Commands;
using CMA.ISMAI.Engine.Domain.EventHandlers;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Logging.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Domain_EngineServiceTest
    {
        [Theory]
        [InlineData(null, "12314124532")]
        [InlineData("ISP", "")]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void EngineService_StartWorkFlow_ShouldFailBecauseEmptyOrNullParameters(string workflowname, string processname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();

            StartWorkFlowCommand Start = new StartWorkFlowCommand(workflowname, processname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startStartCommandHandler.Handle(Start, new CancellationToken()).Result;
            Assert.False(result);
        }
        [Fact]
        public void EngineService_StartWorkFlow_ShouldFailBecauseEmptyOrNullParametersDictionary()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();

            StartWorkFlowCommand Start = new StartWorkFlowCommand("ISP", "12314124532", Assembly.GetExecutingAssembly(), null);
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startStartCommandHandler.Handle(Start, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Fact]
        public void EngineService_StartWorkFlow_ShouldFailBecauseNullAssembly()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();

            StartWorkFlowCommand Start = new StartWorkFlowCommand("ISP", "12314124532", null, new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startStartCommandHandler.Handle(Start, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("ISMAI", "Process_00kjd12")]
        public void EngineService_StartWorkFlow_ShouldReturnStartID(string workflowname, string processname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(Guid.NewGuid().ToString());
            StartWorkFlowCommand Start = new StartWorkFlowCommand(workflowname, processname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startStartCommandHandler.Handle(Start, new CancellationToken()).Result;
            Assert.True(result);
        }

        [Theory]
        [InlineData("ISEP", "Process_00kjdw0")]
        [InlineData("", "Process_00kjdw0")]
        public void EngineService_StartWorkFlow_ShouldFailBecauseNonExistingWorkFlow(string workflowname, string processname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(Guid.NewGuid().ToString());
            StartWorkFlowCommand Start = new StartWorkFlowCommand(workflowname, processname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startStartCommandHandler.Handle(Start, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("ISMAI", "Process_00kjd12")]
        public void EngineService_StartWorkFlowCommand_ShouldFailBecauseStartFailed(string workflowname, string processname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(string.Empty);
            StartWorkFlowCommand Start = new StartWorkFlowCommand(workflowname, processname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startStartCommandHandler.Handle(Start, new CancellationToken()).Result;
            Assert.False(result);
        }


        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("ISMAI", "Process_00kjd12")]
        public void EngineService_StartWorkFlowEvent_ShouldReturnEventCompleted(string workflowname, string processname)
        {
            WorkFlowStartCompletedEvent workflowEvent = new WorkFlowStartCompletedEvent(Guid.NewGuid(), workflowname, processname, true);
            WorkFlowEventHandler startWorkFlowEvent = new WorkFlowEventHandler();

            bool result = startWorkFlowEvent.Handle(workflowEvent, new CancellationToken()).IsCompleted;
            Assert.True(result);
        }
    }
}
