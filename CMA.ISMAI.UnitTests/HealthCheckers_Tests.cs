using CMA.ISMAI.Logging.Interface;
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
        [Fact]
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

        [Fact]
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
    }
}
