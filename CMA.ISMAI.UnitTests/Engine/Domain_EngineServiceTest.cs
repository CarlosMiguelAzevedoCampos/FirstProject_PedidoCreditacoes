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
        public void EngineService_StartWorkFlow_ShouldReturnFalseBecauseDosentHaveFileParameter()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();

            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            string result = engineService.StartWorkFlow(new Deploy(null, guid, true), null);
            Assert.Empty(result);
        }

        [Fact]
        public void EngineService_StartWorkFlow_ShouldReturnEmptyBeucaseDosentHaveProcessName()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns("");

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            string result = engineService.StartWorkFlow(new Deploy(null, "", true), Assembly.GetExecutingAssembly());
            Assert.Empty(result);
        }

        [Fact]
        public void EngineService_StartWorkFlow_ShouldReturnDeployIDForNonCET()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(guid);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);
            string result = engineService.StartWorkFlow(new Deploy("ISMAI", "Process_00kjdw0", false), Assembly.GetExecutingAssembly());
            Assert.Equal(result, guid);
        }

        [Fact]
        public void EngineService_StartWorkFlow_ShouldReturnDeployIDForCET()
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(guid);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);
            string result = engineService.StartWorkFlow(new Deploy("ISMAI", "Process_00kjdw0", true), Assembly.GetExecutingAssembly());
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
