using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service;
using CMA.ISMAI.Logging.Interface;
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
            httprequestMock.Setup(x => x.CardPostAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.CoordenatorExcelAction("12", string.Empty);
            Assert.True(!string.IsNullOrEmpty(result));
        }

        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndFailCreatingaCard()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            httprequestMock.Setup(x => x.CardPostAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<string>())).Returns(Task.FromResult(""));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.CoordenatorExcelAction("12", string.Empty);
            Assert.True(string.IsNullOrEmpty(result));
        }


        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndFail()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            string result = creditacoesService.CoordenatorExcelAction("12", string.Empty);
            Assert.True(string.IsNullOrEmpty(result));
        }

    }
}
