using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.HealthCheck;
using CMA.ISMAI.Trello.API.HealthCheck.Interface;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Solutions.Creditacoes
{
    public class HealthCheckers_Tests
    {
        [Fact(DisplayName ="Health Check the Camunda. Should return Healthy")]
        [Trait("HealthCheck", "Solution HealthCheck")]
        private void CamundaHealthCheck_ApiStatus_ReturnHealthy()
        {
            var httpMock = new Mock<IHttpRequest>();
            var logMock = new Mock<ILog>();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            httpMock.Setup(x => x.MakeAnHttpRequest(It.IsAny<string>())).Returns(Task.FromResult(response));
            CamundaHealthCheck camundaHealthCheck = new CamundaHealthCheck(logMock.Object, httpMock.Object);
            var result = camundaHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>(), new CancellationToken()).Result;
            Assert.True(result.Status == HealthStatus.Healthy);
        }

        [Fact(DisplayName = "Health Check the Camunda. Should return Unhealthy")]
        [Trait("HealthCheck", "Solution HealthCheck")]
        private void CamundaHealthCheck_ApiStatus_ReturnUnhealthy()
        {
            var httpMock = new Mock<IHttpRequest>();
            var logMock = new Mock<ILog>();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpMock.Setup(x => x.MakeAnHttpRequest(It.IsAny<string>())).Returns(Task.FromResult(response));
            CamundaHealthCheck camundaHealthCheck = new CamundaHealthCheck(logMock.Object, httpMock.Object);
            var result = camundaHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>(), new CancellationToken()).Result;
            Assert.True(result.Status == HealthStatus.Unhealthy);
        }

        [Fact(DisplayName = "Health Check the Trello Api. Should return Healthy")]
        [Trait("HealthCheck", "Solution HealthCheck")]
        private void TrelloHealthCheck_ApiStatus_ReturnHealthy()
        {
            var httpMock = new Mock<IHttpRequest>();
            var logMock = new Mock<ILog>();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            httpMock.Setup(x => x.MakeAnHttpRequest(It.IsAny<string>())).Returns(Task.FromResult(response));
            TrelloHealthCheck trelloHealthCheck = new TrelloHealthCheck(logMock.Object, httpMock.Object);
            var result = trelloHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>(), new CancellationToken()).Result;
            Assert.True(result.Status == HealthStatus.Healthy);
        }

        [Fact(DisplayName = "Health Check the Trello Api. Should return Unhealthy")]
        [Trait("HealthCheck", "Solution HealthCheck")]
        private void TrelloHealthCheck_ApiStatus_ReturnUnhealthy()
        {
            var httpMock = new Mock<IHttpRequest>();
            var logMock = new Mock<ILog>();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpMock.Setup(x => x.MakeAnHttpRequest(It.IsAny<string>())).Returns(Task.FromResult(response));
            TrelloHealthCheck trelloHealthCheck = new TrelloHealthCheck(logMock.Object, httpMock.Object);
            var result = trelloHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>(), new CancellationToken()).Result;
            Assert.True(result.Status == HealthStatus.Unhealthy);
        }
    }
}
