using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Service;
using CMA.ISMAI.Sagas.Service.Interface;
using CMA.ISMAI.Sagas.Service.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas
{
    public class CreditacaoService_Tests
    {
        [Fact(DisplayName = "Get Card Status. Should return card completed.")]
        [Trait("SagaService", "Get Card Status")]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatus_ReturnCardCompleted()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardState(It.IsAny<string>())).Returns(Task.FromResult(true));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.GetCardState("12");
            Assert.True(result);
        }

        [Fact(DisplayName = "Get Card Status. Should return card incompleted.")]
        [Trait("SagaService", "Get Card Status")]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatus_ReturnCardIncompleted()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardState(It.IsAny<string>())).Returns(Task.FromResult(false));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.GetCardState("12");
            Assert.False(result);
        }

        [Fact(DisplayName = "Post new Card. Should Fail.")]
        [Trait("SagaService", "Post new Card")]
        public void CreditacoesService_PostCard_ShouldFailToPostTheCard()
        {
            var list = new List<string>();
            list.Add("google.pt");
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardPost(It.IsAny<CardDto>())).Returns(Task.FromResult(string.Empty));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.PostNewCard(new CardDto("Carlos Campos", DateTime.Now.AddDays(1), "Carlos Campos", 1, list,
                "ISMAI", "Informática", "Carlos Campos", false));
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Post new Card. Should Pass.")]
        [Trait("SagaService", "Post new Card")]
        public void CreditacoesService_PostCard_ShouldPostTheCard()
        {
            var list = new List<string>();
            list.Add("google.pt");
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardPost(It.IsAny<CardDto>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.PostNewCard(new CardDto("Carlos Campos", DateTime.Now.AddDays(1), "Carlos Campos", 1, list,
                "ISMAI", "Informática", "Carlos Campos", false));
            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "Get card attchments.")]
        [Trait("SagaService", "Get card attachements")]
        public void CreditacoesService_GetCardAttchments_ShouldGetCardAttachments()
        {
            var list = new List<string>();
            list.Add("google.pt");
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.GetCardAttachments(It.IsAny<string>()))
                .Returns(Task.FromResult(list));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            List<string> result = creditacoesService.GetCardAttachments("12");
            Assert.True(result.Count > 0);
        }

        [Fact(DisplayName = "Delete Card. Should Fail.")]
        [Trait("SagaService", "Delete Card")]
        public void CreditacoesService_DeleteCCard_ShouldFailToDeleteTheCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.DeleteCard(It.IsAny<string>())).Returns(Task.FromResult(false));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.DeleteCard(Guid.NewGuid().ToString());
            Assert.False(result);
        }

        [Fact(DisplayName = "Delete Card. Should Pass.")]
        [Trait("SagaService", "Delete Card")]
        public void CreditacoesService_DeleteCard_ShouldDeleteTheCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.DeleteCard(It.IsAny<string>())).Returns(Task.FromResult(true));
            ISagaService creditacoesService = new CreditacaoService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.DeleteCard(Guid.NewGuid().ToString());
            Assert.True(result);
        }
    }
}
