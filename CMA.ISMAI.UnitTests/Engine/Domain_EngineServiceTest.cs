using CMA.ISMAI.Automation.DomainInterface;
using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Automation.Service;
using CMA.ISMAI.Engine.Domain.Model;
using CMA.ISMAI.Logging.Interface;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Domain_EngineServiceTest
    {
        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnFalseBecauseDosentHaveFileParameter()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();

            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>())).Returns(string.Empty);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            string result = engineService.DeployWorkFlow(new Deploy(null, guid), null);
            Assert.Empty(result);
        }

        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnEmptyBeucaseDosentHaveProcessName()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>())).Returns("");

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            string result = engineService.DeployWorkFlow(new Deploy(null, ""), Assembly.GetExecutingAssembly());
            Assert.Empty(result);
        }

        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnDeployID()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>())).Returns(guid);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);
            string result = engineService.DeployWorkFlow(new Deploy("ISMAI", "Process_00kjdw0"), Assembly.GetExecutingAssembly());
            Assert.Equal(result, guid);
        }

        [Fact]
        public void EngineService_DeleteWorkFlow_ShouldReturnFalseBecauseOfEmptyName()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();

            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(false);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            bool result = engineService.DeleteDeployment("");
            Assert.False(result);
        }

        [Fact]
        public void EngineService_DeleteWorkFlow_ShouldReturnTrue()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();

            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(true);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            bool result = engineService.DeleteDeployment("Process_00kjdw0");
            Assert.True(result);
        }
    }
}
