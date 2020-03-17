using CMA.ISMAI.Engine.API.Controllers;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Controllers;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Controller_TrelloServiceTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrelloService_DeleteWorkFlow_ShouldReturnBadRequestBecauseOfEmptyOrNullParameters(string id)
        {
            var engineMock = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(false));

            TrelloController trelloController = new TrelloController(logMock.Object, engineMock.Object);

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Theory]
        [InlineData("ifh2i992h2b-asfa-1w", false)]
        [InlineData("cjckamrb222-we-1w", true)]
        public void TrelloService_DeleteWorkFlow_ShouldReturnOkStatus(string id, bool resultSetup)
        {
            var engineMock = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(resultSetup));

            TrelloController trelloController = new TrelloController(logMock.Object, engineMock.Object);

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

        [Theory]
        [InlineData(null, "process_q134")]
        [InlineData("ISP", "")]
        [InlineData(null, "Process_qe332")]
        [InlineData("ISMAI", "")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void TrelloService_AddCard_ShouldReturnBadStatusBecauseOfNullOrEmptyParameters(string cardName, string cardDescription)
        {
            var engineMock = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.AddCard(It.IsAny<Card>())).Returns(Task.FromResult(string.Empty));

            TrelloController trelloController = new TrelloController(logMock.Object, engineMock.Object);

            IActionResult result = trelloController.AddCard(new CardDto(cardName, DateTime.Now, cardDescription));
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Fact]
        public void TrelloController_AddCard_ShouldReturnBadStatusBecauseOfNullDto()
        {
            var engineMock = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();

            TrelloController trelloController = new TrelloController(logMock.Object, engineMock.Object);

            IActionResult result = trelloController.AddCard(null);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }



        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("FEUP", "Process_00kjdw0")]
        [InlineData("ISLA", "Process_00kjdw0")]
        [InlineData("ISP", "Process_00kjd12")]
        public void TrelloController_AddCard_ShouldReturnOkStatus(string cardName, string cardDescription)
        {
            var engineMock = new Mock<ITrelloService>();
            var logMock = new Mock<ILog>();
            engineMock.Setup(x => x.AddCard(It.IsAny<Card>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));

            TrelloController trelloController = new TrelloController(logMock.Object, engineMock.Object);

            IActionResult result = trelloController.AddCard(new CardDto(cardName, DateTime.Now, cardDescription));
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }
    }
}
