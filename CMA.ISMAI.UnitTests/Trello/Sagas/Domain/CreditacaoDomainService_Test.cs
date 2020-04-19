using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Service.Creditacao;
using CMA.ISMAI.Sagas.Service.Interface;
using CMA.ISMAI.Sagas.Service.Model;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.Domain
{
    public class CreditacaoDomainService_Test
    {
        [Trait("Creditação Service", "Card creation")]
        [Theory(DisplayName ="Get attachmnents from a card and create a new card with the same attachments.")]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", true, 1)]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", false, 1)]
        public void Creditacao_CreditacaoWithNewCardCreation_ShouldGetCardAttachmentsAndCreateOneCard(string cardId, string courseName, string studentName, string courseInstitute, bool IsCetOrOtherCondition, int boardId)
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>());
            sagaMock.Setup(x => x.PostNewCard(It.IsAny<CardDto>())).Returns(Guid.NewGuid().ToString());
            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(true);

            CreditacaoDomainService creditacaoDomain = new CreditacaoDomainService(sagaMock.Object);
            string value = creditacaoDomain.CreateNewCard(cardId, courseName, studentName, courseInstitute, DateTime.Now.AddDays(2), IsCetOrOtherCondition, boardId);
            Assert.NotEmpty(value);
        }

        [Trait("Creditação Service", "Card creation")]
        [Theory(DisplayName = "Get attachmnents from a card and fail creating a new card with the same attachments.")]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", true, 1)]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", false, 1)]
        public void Creditacao_CreditacaoWithNewCardCreation_FailOnCardCreation(string cardId, string courseName, string studentName, string courseInstitute, bool IsCetOrOtherCondition, int boardId)
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>());
            sagaMock.Setup(x => x.PostNewCard(It.IsAny<CardDto>())).Returns(string.Empty);
            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(false);

            CreditacaoDomainService creditacaoDomain = new CreditacaoDomainService(sagaMock.Object);
            string value = creditacaoDomain.CreateNewCard(cardId, courseName, studentName, courseInstitute, DateTime.Now.AddDays(2), IsCetOrOtherCondition, boardId);
            Assert.Empty(value);
        }

        [Trait("Creditação Service", "Card Behavior")]
        [Fact(DisplayName = "Test the card status, should return incompleted")]
        public void Creditacao_GetCardStatus_ReturnCardStatus_CardNotCompleted()
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(false);
            CreditacaoDomainService creditacaoDomain = new CreditacaoDomainService(sagaMock.Object);
            bool value = creditacaoDomain.GetCardStatus("jffh8ywnnnojsob");

            Assert.False(value);
        }

        [Trait("Creditação Service", "Card Behavior")]
        [Fact(DisplayName = "Test the card status, should return completed")]
        public void Creditacao_GetCardStatus_ReturnCardStatus_CardCompleted()
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(true);
            CreditacaoDomainService creditacaoDomain = new CreditacaoDomainService(sagaMock.Object);
            bool value = creditacaoDomain.GetCardStatus("jffh8ywnnnojsob");

            Assert.True(value);
        }

        [Fact(DisplayName = "Summer break is activated and it's time to delay activities")]
        [Trait("SagaService", "Summer break condition")]
        public void CreditacoesService_ISummerBreakActivated_ShouldReturnTheOptionState()
        {
            var sagaMock = new Mock<ISagaService>();
            sagaMock.Setup(x => x.IsSummerBreakTime()).Returns(true);
            CreditacaoDomainService creditacaoDomain = new CreditacaoDomainService(sagaMock.Object);
            bool result = creditacaoDomain.IsSummerBreakTime(8);
            Assert.True(result);
        }

        [Fact(DisplayName = "Summer break is activated and it's July. No delay will be activated")]
        [Trait("Creditação Service", "Summer break condition")]
        public void CreditacoesService_IsTimeForSummerBreak_ShouldReturnFalse()
        {
            var sagaMock = new Mock<ISagaService>();
            sagaMock.Setup(x => x.IsSummerBreakTime()).Returns(true);
            CreditacaoDomainService creditacaoDomain = new CreditacaoDomainService(sagaMock.Object);
            bool result = creditacaoDomain.IsSummerBreakTime(7);
            Assert.False(result);
        }

        [Fact(DisplayName = "Delete Card. Should Fail.")]
        [Trait("Creditação Service", "Delete Card")]
        public void CreditacoesService_DeleteCCard_ShouldFailToDeleteTheCard()
        {
            var sagaMock = new Mock<ISagaService>();
            sagaMock.Setup(x => x.DeleteCard(It.IsAny<string>())).Returns(false);
            CreditacaoDomainService creditacoesService = new CreditacaoDomainService(sagaMock.Object);
            bool result = creditacoesService.DeleteCard(Guid.NewGuid().ToString());
            Assert.False(result);
        }

        [Fact(DisplayName = "Delete Card. Should Pass.")]
        [Trait("Creditação Service", "Delete Card")]
        public void CreditacoesService_DeleteCard_ShouldDeleteTheCard()
        {
            var sagaMock = new Mock<ISagaService>();
            sagaMock.Setup(x => x.DeleteCard(It.IsAny<string>())).Returns(true);
            CreditacaoDomainService creditacoesService = new CreditacaoDomainService(sagaMock.Object);
            bool result = creditacoesService.DeleteCard(Guid.NewGuid().ToString());
            Assert.True(result);
        }
    }
}
