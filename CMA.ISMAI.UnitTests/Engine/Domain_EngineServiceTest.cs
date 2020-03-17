using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Domain.CommandHandlers;
using CMA.ISMAI.Engine.Domain.Commands;
using CMA.ISMAI.Logging.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
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
        public void EngineService_DeployWorkFlow_ShouldFailBecauseEmptyOrNullParameters(string workflowname, string processname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();

            StartDeployCommand deploy = new StartDeployCommand(workflowname, processname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            StartDeployCommandHandler startDeployCommandHandler = new StartDeployCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startDeployCommandHandler.Handle(deploy, new CancellationToken()).Result;
            Assert.False(result);
        }
        [Fact]
        public void EngineService_DeployWorkFlow_ShouldFailBecauseEmptyOrNullParametersDictionary()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();
            string guid = Guid.NewGuid().ToString();

            StartDeployCommand deploy = new StartDeployCommand("ISP", "12314124532", Assembly.GetExecutingAssembly(), null);
            StartDeployCommandHandler startDeployCommandHandler = new StartDeployCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startDeployCommandHandler.Handle(deploy, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Fact]
        public void EngineService_DeployWorkFlow_ShouldFailBecauseNullAssembly()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();
            string guid = Guid.NewGuid().ToString();

            StartDeployCommand deploy = new StartDeployCommand("ISP", "12314124532", null, new Dictionary<string, object>());
            StartDeployCommandHandler startDeployCommandHandler = new StartDeployCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startDeployCommandHandler.Handle(deploy, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("ISP", "Process_00kjd12")]
        public void EngineService_DeployWorkFlow_ShouldReturnDeployID(string workflowname, string processname)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);
            StartDeployCommand deploy = new StartDeployCommand(workflowname, processname, Assembly.GetExecutingAssembly(), new Dictionary<string, object>());
            StartDeployCommandHandler startDeployCommandHandler = new StartDeployCommandHandler(busMock.Object, new DomainNotificationHandler(), engineMock.Object, logMock.Object);

            bool result = startDeployCommandHandler.Handle(deploy, new CancellationToken()).Result;
            Assert.True(result);
        }
    }
}
