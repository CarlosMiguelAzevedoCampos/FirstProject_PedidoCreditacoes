using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Controllers;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.ComponentModel;
using Xunit;

namespace CMA.ISMAI.UnitTests.Solutions.Creditacoes
{
    public class CreditacaoController_Tests
    {
        [Fact(DisplayName ="Create a new card and starts a new Process")]
        [Trait("Creditação Solutions", "Controller")]
        private void CreditacaoController_NewCreditacaoPost_CreateNewCardOnTrello_AndStartsTheProcess()
        {
            var trelloService = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            trelloService.Setup(x => x.CreateTrelloCard(It.IsAny<CreditacaoDto>())).Returns(true);

            CreditacaoController creditacaoController = new CreditacaoController(trelloService.Object, logMock.Object)
            {
                TempData = tempData
            };
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCetOrOtherCondition = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);

            string result = tempData["Process_status"].ToString();
            Assert.True(result == "Processo Criado com Sucesso!");
        }

        [Fact(DisplayName = "Fail on creating a new card and on creating Process")]
        [Trait("Creditação Solutions", "Controller")]
        private void CreditacaoController_NewCreditacaoPost_FailOnCardAndProcessCreation()
        {
            var trelloService = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            trelloService.Setup(x => x.CreateTrelloCard(It.IsAny<CreditacaoDto>())).Returns(false);
          
            CreditacaoController creditacaoController = new CreditacaoController(trelloService.Object, logMock.Object)
            {
                TempData = tempData
            };
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCetOrOtherCondition = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);

            string result = tempData["Process_status"].ToString(); 
            Assert.True(result == "Ocorreu um erro!, porfavor, contacte o IT");
        }


        [Fact(DisplayName = "Creditação Controller - Invalid Model")]
        [Trait("Creditação Solutions", "Controller")]
        private void CreditacaoController_NewCreditacaoPost_InvalidModel()
        {
            var trelloService = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            trelloService.Setup(x => x.CreateTrelloCard(It.IsAny<CreditacaoDto>())).Returns(false);

            CreditacaoController creditacaoController = new CreditacaoController(trelloService.Object, logMock.Object)
            {
                TempData = tempData
                
            };
            creditacaoController.ModelState.AddModelError("Invalid Request", "Invalid request");
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCetOrOtherCondition = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);

            string result = tempData["Process_status"].ToString();
            Assert.True(result == "Ocorreu um erro!, porfavor, verifique os seus campos");
        }
    }
}
