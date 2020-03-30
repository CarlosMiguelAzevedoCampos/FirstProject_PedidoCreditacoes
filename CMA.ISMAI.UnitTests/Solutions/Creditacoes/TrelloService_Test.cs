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
        [Fact]
        private void TrelloService_CreateTrelloCard_CreateCardAndReturnId()
        {
            var httpMock = new Mock<IHttpRequest>();
            httpMock.Setup(x => x.PostNewCardAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            ITrelloService trelloService = new TrelloService(httpMock.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            string value = trelloService.CreateTrelloCard(creditacaoDto);
            Assert.NotEmpty(value);
        }

        [Fact]
        private void TrelloService_CreateTrelloCard_ReturnAnEmptyCardId()
        {
            var httpMock = new Mock<IHttpRequest>();
            httpMock.Setup(x => x.PostNewCardAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(string.Empty));
            ITrelloService trelloService = new TrelloService(httpMock.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            string value = trelloService.CreateTrelloCard(creditacaoDto);
            Assert.Empty(value);
        }
    }
}
