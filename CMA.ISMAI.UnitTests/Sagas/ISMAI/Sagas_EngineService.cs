using CamundaClient.Dto;
using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI;
using CMA.ISMAI.Logging.Interface;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.ISMAI
{
    public class Sagas_EngineService
    {
        [Fact]
        public void SagasISMAI_RegisterNewWorker_ShouldAddToTheListTheWorkers()
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<IEngine>();

            ISaga creditacaoSaga = new CreditacaoSaga(engineMock.Object, logMock.Object);
            creditacaoSaga.RegisterNewWorker();
            IDictionary<string, Action<ExternalTask>> workers = creditacaoSaga.ReturnExternalWorkersTasks();
            Assert.True(workers.Count > 0);
        }
    }
}
