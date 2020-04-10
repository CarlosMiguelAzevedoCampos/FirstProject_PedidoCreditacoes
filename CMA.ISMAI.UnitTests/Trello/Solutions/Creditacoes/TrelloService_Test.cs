using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Solutions.Creditacoes
{
    public class TrelloService_Test
    {
        [Fact(DisplayName = "Create a new card and return is Id")]
        [Trait("Creditação Solutions", "Trello Service")]
        private void TrelloService_CreateTrelloCard_CreateCardAndReturnId()
        {
            var httpMock = new Mock<IHttpRequest>();
            var logMock = new Mock<ILog>();
            httpMock.Setup(x => x.PostNewCardAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(true));

            ITrelloService trelloService = new TrelloService(httpMock.Object, logMock.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            bool value = trelloService.CreateTrelloCard(creditacaoDto);
            Assert.True(value);
        }

        [Fact(DisplayName = "Creditação Solution - Trello Service - Return error on creating a new Card")]
        [Trait("Creditação Solutions", "Trello Service")]
        private void TrelloService_CreateTrelloCard_ReturnFalseOnNewCardCreation()
        {
            var httpMock = new Mock<IHttpRequest>();
            var logMock = new Mock<ILog>();
            httpMock.Setup(x => x.PostNewCardAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(false));

            ITrelloService trelloService = new TrelloService(httpMock.Object, logMock.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            bool value = trelloService.CreateTrelloCard(creditacaoDto);
            Assert.False(value);
        }
    }
}
