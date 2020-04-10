using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using CMA.ISMAI.Sagas.Services.Service;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.ISMAI
{
    public class CreditacaoService_Test
    {
        [Trait("Creditação Service", "Card creation")]
        [Theory(DisplayName ="Get attachmnents from a card and create a new card with the same attachments.")]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", true, 1)]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", false, 1)]
        public void Creditacao_CreditacaoWithNewCardCreation_ShouldGetCardAttachmentsAndCreateOneCard(string cardId, string courseName, string studentName, string courseInstitute, bool isCet, int boardId)
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>());
            sagaMock.Setup(x => x.PostNewCard(It.IsAny<CardDto>())).Returns(Guid.NewGuid().ToString());
            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(true);

            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            string value = creditacaoService.CreditacaoWithNewCardCreation(cardId, courseName, studentName, courseInstitute, DateTime.Now.AddDays(2), isCet, boardId);
            Assert.NotEmpty(value);
        }

        [Trait("Creditação Service", "Card creation")]
        [Theory(DisplayName = "Get attachmnents from a card and fail creating a new card with the same attachments.")]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", true, 1)]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", false, 1)]
        public void Creditacao_CreditacaoWithNewCardCreation_FailOnCardCreation(string cardId, string courseName, string studentName, string courseInstitute, bool isCet, int boardId)
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>());
            sagaMock.Setup(x => x.PostNewCard(It.IsAny<CardDto>())).Returns(string.Empty);
            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(false);

            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            string value = creditacaoService.CreditacaoWithNewCardCreation(cardId, courseName, studentName, courseInstitute, DateTime.Now.AddDays(2), isCet, boardId);
            Assert.Empty(value);
        }

        [Trait("Creditação Service", "Card Behavior")]
        [Fact(DisplayName = "Test the card status, should return incompleted")]
        public void Creditacao_GetCardStatus_ReturnCardStatus_CardNotCompleted()
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(false);
            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            bool value = creditacaoService.GetCardStatus("jffh8ywnnnojsob");

            Assert.False(value);
        }

        [Trait("Creditação Service", "Card Behavior")]
        [Fact(DisplayName = "Test the card status, should return completed")]
        public void Creditacao_GetCardStatus_ReturnCardStatus_CardCompleted()
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(true);
            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            bool value = creditacaoService.GetCardStatus("jffh8ywnnnojsob");

            Assert.True(value);
        }
    }
}
