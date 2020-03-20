using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Engine.Domain.CommandHandlers;
using CMA.ISMAI.Engine.Domain.Commands;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Engine.Domain.Interface;
using CMA.ISMAI.Logging.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Domain_EngineServiceTest
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EngineService_StartWorkFlow_ShouldFailBecauseEmptyOrNullParameters(string workflowname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();

            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand(workflowname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartFailedEvent);
        }


        [Fact]
        public void EngineService_StartWorkFlow_ShouldFailBecauseEmptyOrNullParametersDictionary()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();

            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand("ISP", Assembly.GetExecutingAssembly(), null);
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartFailedEvent);
        }

        [Fact]
        public void EngineService_StartWorkFlow_ShouldFailBecauseOfAnNullAssembly()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();


            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand("ISP", null, new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartFailedEvent);
        }

        [Theory]
        [InlineData("ISMAI")]
        public void EngineService_StartWorkFlow_ShouldReturnStartID(string workflowname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();

            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(Guid.NewGuid().ToString());
            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand(workflowname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartCompletedEvent);
        }

        [Theory]
        [InlineData("ISEP")]
        [InlineData("")]
        public void EngineService_StartWorkFlow_ShouldFailBecauseNonExistingWorkFlow(string workflowname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(Guid.NewGuid().ToString());
            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand(workflowname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartFailedEvent);
        }

        [Theory]
        [InlineData("ISMAI")]
        public void EngineService_StartWorkFlowCommand_ShouldFailBecauseTheEngineFailed(string workflowname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(string.Empty);
            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand(workflowname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartFailedEvent);
        }


        [Theory]
        [InlineData("ISMAI")]
        public void EngineService_StartWorkFlowEvent_ShouldReturnEventCompleted(string workflowname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var workFlowMock = new Mock<IWorkflowEventHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(Guid.NewGuid().ToString());
            StartWorkFlowCommand startWorkFlowCommand = new StartWorkFlowCommand(workflowname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            WorkFlowCommandHandler startStartCommandHandler = new WorkFlowCommandHandler(engineMock.Object, logMock.Object, workFlowMock.Object);

            Event result = startStartCommandHandler.Handle(startWorkFlowCommand);
            Assert.True(result is WorkFlowStartCompletedEvent);
        }
    }
}
