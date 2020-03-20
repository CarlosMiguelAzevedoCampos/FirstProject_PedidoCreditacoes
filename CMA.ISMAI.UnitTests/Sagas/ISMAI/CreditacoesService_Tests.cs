using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service;
using CMA.ISMAI.Logging.Interface;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.ISMAI
{
    public class CreditacoesService_Tests
    {
        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndComplete()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.CoordenatorExcelAction("12", string.Empty);
            Assert.True(result);
        }

        [Fact]
        public void CreditacoesService_GetCardStatus_ShouldGetCardStatusAndNotify()
        {
            var logMock = new Mock<ILog>();
            var httprequestMock = new Mock<IHttpRequest>();
            httprequestMock.Setup(x => x.CardStateAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            ICreditacoesService creditacoesService = new CreditacoesService(logMock.Object, httprequestMock.Object);
            bool result = creditacoesService.CoordenatorExcelAction("12", string.Empty);
            Assert.False(result);
        }

    }
}
