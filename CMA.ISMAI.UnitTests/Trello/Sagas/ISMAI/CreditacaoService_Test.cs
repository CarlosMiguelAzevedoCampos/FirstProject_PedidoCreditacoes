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
        [Theory]
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

        [Theory]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", true, 1)]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", false, 1)]
        public void Creditacao_CreditacaoWithNewCardCreation_CardIsntCompleted(string cardId, string courseName, string studentName, string courseInstitute, bool isCet, int boardId)
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(false);

            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            string value = creditacaoService.CreditacaoWithNewCardCreation(cardId, courseName, studentName, courseInstitute, DateTime.Now.AddDays(2), isCet, boardId);
            Assert.Empty(value);
        }

        [Theory]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", true, 1)]
        [InlineData("jffh8ywnnnojsob", "Informática", "Carlos Campos", "ISMAI", false, 1)]
        public void Creditacao_CreditacaoWithNewCardCreation_FailOnCardCreation(string cardId, string courseName, string studentName, string courseInstitute, bool isCet, int boardId)
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>());
            sagaMock.Setup(x => x.PostNewCard(It.IsAny<CardDto>())).Returns(string.Empty);
            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(true);
            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            string value = creditacaoService.CreditacaoWithNewCardCreation(cardId, courseName, studentName, courseInstitute, DateTime.Now.AddDays(2), isCet, boardId);
            Assert.Empty(value);
        }

        [Fact]
        public void Creditacao_CreditacaoWithNoNewCardCreation_ReturnCardStatus_CardNotCompleted()
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(false);
            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            bool value = creditacaoService.CreditacaoWithNoCardCreation("jffh8ywnnnojsob");
            Assert.False(value);
        }

        [Fact]
        public void Creditacao_CreditacaoWithNoNewCardCreation_ReturnCardStatus_CardCompleted()
        {
            var logMock = new Mock<ILog>();
            var sagaMock = new Mock<ISagaService>();

            sagaMock.Setup(x => x.GetCardState(It.IsAny<string>())).Returns(true);
            CreditacaoService creditacaoService = new CreditacaoService(sagaMock.Object);
            bool value = creditacaoService.CreditacaoWithNoCardCreation("jffh8ywnnnojsob");
            Assert.True(value);
        }
    }
}
