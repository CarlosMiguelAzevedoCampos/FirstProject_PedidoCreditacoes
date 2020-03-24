using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.ISMAI
{
    public class CreditacoesService_Tests
    {
        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndCreateOneCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.GetCardState("12");
            Assert.True(result);
        }

        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndFailCreatingaCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            httprequestMock.Setup(x => x.CardPostAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(""));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.GetCardState("12");
            Assert.False(result);
        }


        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndFail()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.GetCardState("12");
            Assert.False(result);
        }

        [Fact]
        public void CreditacoesService_PostCard_ShouldFailToPostTheCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardPostAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(string.Empty));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.PostNewCard(new CardDto("Carlos Campos", DateTime.Now.AddDays(1),1, "Carlos Campos"));
            Assert.Empty(result);
        }

        [Fact]
        public void CreditacoesService_PostCard_ShouldPostTheCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardPostAsync(It.IsAny<CardDto>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.PostNewCard(new CardDto("Carlos Campos", DateTime.Now.AddDays(1), 1, "Carlos Campos"));
            Assert.NotEmpty(result);
        }
    }
}
