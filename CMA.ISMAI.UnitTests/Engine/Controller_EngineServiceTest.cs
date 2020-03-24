using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.API.Controllers;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Engine.Domain.Commands;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Engine.Domain.Interface;
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
            var workFlowMock = new Mock<IWorkflowCommandHandler>();

            EngineController engineController = new EngineController(logMock.Object, workFlowMock.Object);

            IActionResult result = engineController.StartWorkFlow(null);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }



        [Theory]
        [InlineData("ISMAI")]
        public void EngineService_StartWorkFlow_ShouldReturnOkStatus(string workflowName)
        {
            var logMock = new Mock<ILog>();
            var workFlowMock = new Mock<IWorkflowCommandHandler>();
            workFlowMock.Setup(x => x.Handle(It.IsAny<StartWorkFlowCommand>())).Returns(new WorkFlowStartCompletedEvent(
                It.IsAny<string>(), It.IsAny<string>()));
            EngineController engineController = new EngineController(logMock.Object, workFlowMock.Object);

            IActionResult result = engineController.StartWorkFlow(new DeployDto(workflowName, new Dictionary<string, object>()));
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

        [Theory]
        [InlineData("ISMAI")]
        public void EngineService_StartWorkFlow_ShouldReturnBadStatusBecauseOfEngineFail(string workflowName)
        {
            var logMock = new Mock<ILog>();
            var workFlowMock = new Mock<IWorkflowCommandHandler>();
            workFlowMock.Setup(x => x.Handle(It.IsAny<StartWorkFlowCommand>()))
                .Returns(new WorkFlowStartFailedEvent(new List<DomainNotification>()));
                
            EngineController engineController = new EngineController(logMock.Object, workFlowMock.Object);

            IActionResult result = engineController.StartWorkFlow(new DeployDto(workflowName, new Dictionary<string, object>()));
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }
    }
}
