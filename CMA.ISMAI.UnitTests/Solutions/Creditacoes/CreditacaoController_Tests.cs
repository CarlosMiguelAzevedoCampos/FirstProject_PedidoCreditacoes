using CMA.ISMAI.Solutions.Creditacoes.UI.Controllers;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace CMA.ISMAI.UnitTests.Solutions.Creditacoes
{
    public class CreditacaoController_Tests
    {
        [Fact]
        private void CreditacaoController_NewCreditacaoPost_CreateNewCardOnTrelloAndOnCamunda()
        {
            var trelloService = new Mock<ITrelloService>();
            var workFlowService = new Mock<IWorkFlowService>();
            trelloService.Setup(x => x.CreateTrelloCard(It.IsAny<CreditacaoDto>())).Returns(Guid.NewGuid().ToString());
            workFlowService.Setup(x => x.CreateWorkFlowProcess(It.IsAny<CreditacaoDto>(), It.IsAny<string>()))
                .Returns(true);
            CreditacaoController creditacaoController = new CreditacaoController(trelloService.Object, workFlowService.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);
            var resultCode = action as OkObjectResult;
            Assert.True(resultCode is OkObjectResult);
        }

        [Fact]
        private void CreditacaoController_NewCreditacaoPost_FailOnCardCreation()
        {
            var trelloService = new Mock<ITrelloService>();
            var workFlowService = new Mock<IWorkFlowService>();
            trelloService.Setup(x => x.CreateTrelloCard(It.IsAny<CreditacaoDto>())).Returns("");
            CreditacaoController creditacaoController = new CreditacaoController(trelloService.Object, workFlowService.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);
            var resultCode = action as BadRequestObjectResult;
            Assert.True(resultCode is BadRequestObjectResult);
        }

        [Fact]
        private void CreditacaoController_NewCreditacaoPost_FailOnWorkFlowDeploy()
        {
            var trelloService = new Mock<ITrelloService>();
            var workFlowService = new Mock<IWorkFlowService>();
            workFlowService.Setup(x => x.CreateWorkFlowProcess(It.IsAny<CreditacaoDto>(), It.IsAny<string>()))
                .Returns(false);
            trelloService.Setup(x => x.CreateTrelloCard(It.IsAny<CreditacaoDto>())).Returns(Guid.NewGuid().ToString());
            CreditacaoController creditacaoController = new CreditacaoController(trelloService.Object, workFlowService.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);
            var resultCode = action as BadRequestObjectResult;
            Assert.True(resultCode is BadRequestObjectResult);
        }
    }
}
