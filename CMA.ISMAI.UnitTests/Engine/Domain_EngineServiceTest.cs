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
        [Theory]
        [InlineData(null, "12314124532", true)]
        [InlineData("ISP", "", true)]
        [InlineData(null, null, false)]
        [InlineData("", "", true)]
        public void EngineService_StartWorkFlow_ShouldFailBecauseEmptyOrNullParameters(string workflowname, string processname, bool isCet)
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            Deploy deploy = new Deploy(workflowname, processname, isCet);

            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            string result = engineService.StartWorkFlow(deploy, Assembly.GetExecutingAssembly());
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EngineService_StartWorkFlow_ShouldFailBecauseNullAssembly(bool isCet)
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();

            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(string.Empty);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            string result = engineService.StartWorkFlow(new Deploy("ISMAI_0210", "INF_P325", isCet), null);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0", true)]
        [InlineData("ISP", "Process_00kjd12", false)]
        public void EngineService_StartWorkFlow_ShouldReturnDeployID(string workflowname, string processname, bool isCet)
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<string>(), It.IsAny<Assembly>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(guid);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);
            string result = engineService.StartWorkFlow(new Deploy(workflowname, processname,isCet), Assembly.GetExecutingAssembly());
            Assert.Equal(result, guid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EngineService_DeleteWorkFlow_ShouldReturnFailBecauseOfNullOrEMptyParameters(string id)
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();

            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(false);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            bool result = engineService.DeleteDeployment(id);
            Assert.False(result);
        }

        [Theory]
        [InlineData("Process_00kjd12")]
        [InlineData("Process_00ds")]
        public void EngineService_DeleteWorkFlow_ShouldReturnTrue(string id)
        {
            var engineMock = new Mock<IEngine>();
            var logMock = new Mock<ILog>();

            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(true);

            IEngineService engineService = new EngineService(engineMock.Object, logMock.Object);

            bool result = engineService.DeleteDeployment(id);
            Assert.True(result);
        }
    }
}
