using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service;
using CMA.ISMAI.Sagas.Service.Interface;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Sagas
{
    public class CreditacaoFinalStepDomain_Test
    {
        [Fact]
        public void CreditacaoFinalStepDomain_FinishProcess_FinishTheProcessAndSendNotification()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomain>();
            var taskProcessingMock = new Mock<ITaskProcessing>();
            var sagaNotificationMock = new Mock<ISagaNotification>();

            creditacaoDomainMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>() { "http://google.pt" });
            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(true);
            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");
            CreditacaoFinalStepDomain creditacaoFinalStepDomain = new CreditacaoFinalStepDomain(creditacaoDomainMock.Object, taskProcessingMock.Object,
                logMock.Object, sagaNotificationMock.Object);
            bool result = creditacaoFinalStepDomain.FinishProcess("ISMAI", new ExternalTask());
            creditacaoDomainMock.Verify(x => x.GetCardAttachments(It.IsAny<string>()), Times.Once);
            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(2));
            taskProcessingMock.Verify(x=>x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void CreditacaoFinalStepDomain_FinishProcess_FailToFinishTheProcess()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomain>();
            var taskProcessingMock = new Mock<ITaskProcessing>();
            var sagaNotificationMock = new Mock<ISagaNotification>();

            creditacaoDomainMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>() { "http://google.pt" });
            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(false);
            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");
            CreditacaoFinalStepDomain creditacaoFinalStepDomain = new CreditacaoFinalStepDomain(creditacaoDomainMock.Object, taskProcessingMock.Object,
                logMock.Object, sagaNotificationMock.Object);
            bool result = creditacaoFinalStepDomain.FinishProcess("ISMAI", new ExternalTask());
            creditacaoDomainMock.Verify(x => x.GetCardAttachments(It.IsAny<string>()), Times.Once);
            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(2));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.False(result);
        }
    }
}
