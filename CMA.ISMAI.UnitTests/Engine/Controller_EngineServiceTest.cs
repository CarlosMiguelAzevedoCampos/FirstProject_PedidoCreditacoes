using CMA.ISMAI.Automation.DomainInterface;
using CMA.ISMAI.Engine.API.Controllers;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Engine.Domain.Model;
using CMA.ISMAI.Logging.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Reflection;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Controller_EngineServiceTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EngineService_DeleteWorkFlow_ShouldReturnBadRequestBecauseOfEmptyOrNullParameters(string id)
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(false);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeleteDeployment(id);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData("i939282h")]
        [InlineData("ISMAI_Process")]
        [InlineData("ISEP_Process")]
        public void EngineService_DeleteWorkFlow_ShouldReturnOkStatus(string id)
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(true);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeleteDeployment(id);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData(null, "process_q134", true)]
        [InlineData("ISMAI", "", false)]
        [InlineData("", "", false)]
        [InlineData(null, null, true)]
        public void EngineService_StartWorkFlow_ShouldReturnBadStatusBecauseOfNullOrEmptyParameters(string workflowName,string processName, bool isCet)
        {
            DeployDto deployDto = new DeployDto(workflowName, processName, isCet);
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(string.Empty);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.StartWorkFlow(deployDto);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }

        [Fact]
        public void EngineService_StartWorkFlow_ShouldReturnBadStatusBecauseOfNullDto()
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(string.Empty);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.StartWorkFlow(null);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }



        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0", true)]
        [InlineData("FEUP", "Process_00kjdw0", true)]
        [InlineData("ISLA", "Process_00kjdw0", true)]
        [InlineData("ISP", "Process_00kjd12", false)]
        public void EngineService_StartWorkFlow_ShouldReturnOkStatus(string workflowName, string processName, bool isCet)
        {
            DeployDto deployDto = new DeployDto(workflowName, processName, isCet);
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.StartWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(Guid.NewGuid().ToString());

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.StartWorkFlow(deployDto);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }
    }
}
