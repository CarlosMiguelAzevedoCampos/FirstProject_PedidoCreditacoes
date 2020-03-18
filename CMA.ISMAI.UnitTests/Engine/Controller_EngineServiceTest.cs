using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.API.Controllers;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Logging.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Controller_EngineServiceTest
    {
        [Fact]
        public void EngineService_StartWorkFlow_ShouldReturnBadStatusBecauseOfNullDto()
        {
            var logMock = new Mock<ILog>();
            var busMock = new Mock<IMediatorHandler>();

            EngineController engineController = new EngineController(logMock.Object, busMock.Object, new DomainNotificationHandler());

            IActionResult result = engineController.StartWorkFlow(null);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }



        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("ISMAI", "Process_00kjd12")]
        public void EngineService_StartWorkFlow_ShouldReturnOkStatus(string workflowName, string processName)
        {
            var logMock = new Mock<ILog>();
            var busMock = new Mock<IMediatorHandler>();

            EngineController engineController = new EngineController(logMock.Object, busMock.Object, new DomainNotificationHandler());

            IActionResult result = engineController.StartWorkFlow(new DeployDto(workflowName, processName, new Dictionary<string, object>()));
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }
    }
}
