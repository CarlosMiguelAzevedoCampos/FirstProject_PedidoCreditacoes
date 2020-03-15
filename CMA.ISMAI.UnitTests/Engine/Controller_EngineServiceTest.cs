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
        [Fact]
        public void EngineService_DeleteWorkFlow_ShouldReturnBadRequestBecauseOfTheEmptyName()
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(false);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeleteDeployment("");
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }

        [Fact]
        public void EngineService_DeleteWorkFlow_ShouldReturnOkStatus()
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeleteDeployment(It.IsAny<string>())).Returns(true);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeleteDeployment("Process_xr1854");
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }

        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnBadStatusBecauseOfEngineReturnFail()
        {
            DeployDto deployDto = new DeployDto("ISMAI", "Process_00kjdw0");
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(string.Empty);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeployWorkFlow(deployDto);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }


        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnOkStatus()
        {
            DeployDto deployDto = new DeployDto("ISMAI", "Process_00kjdw0");
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(Guid.NewGuid().ToString());

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeployWorkFlow(deployDto);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }


        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnBadStatusBecauseOfMissingValues()
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(string.Empty);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeployWorkFlow(new DeployDto("", "Process_00kjdw0"));
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }

        [Fact]
        public void EngineService_DeployWorkFlow_ShouldReturnBadStatusBecauseOfEngineServiceReturnValue()
        {
            var engineMock = new Mock<IEngineService>();
            var logMock = new Mock<ILog>();
            string guid = Guid.NewGuid().ToString();
            engineMock.Setup(x => x.DeployWorkFlow(It.IsAny<Deploy>(), It.IsAny<Assembly>())).Returns(string.Empty);

            EngineController engineController = new EngineController(logMock.Object, engineMock.Object);

            IActionResult result = engineController.DeployWorkFlow(new DeployDto("", "Process_00kjdw0"));
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
            Response response = JsonConvert.DeserializeObject<Response>(resultCode.Value.ToString());
            Assert.NotNull(response);
        }
    }
}
