using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Controllers;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using Xunit;

namespace CMA.ISMAI.UnitTests.Solutions.Creditacoes
{
    public class CreditacaoController_Tests
    {
        [Fact]
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
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);

            string result = tempData["Process_status"].ToString();
            Assert.True(result == "Process deployed!");
        }
  
        [Fact]
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
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);

            string result = tempData["Process_status"].ToString(); 
            Assert.True(result == "An error happend while creating the process!, please contact the IT!");
        }


        [Fact]
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
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            IActionResult action = creditacaoController.Create(creditacaoDto);

            string result = tempData["Process_status"].ToString();
            Assert.True(result == "An error happend while creating the process!, invalid model!");
        }
    }
}
