using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service.Creditacao;
using CMA.ISMAI.Sagas.Service.Interface;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.Domain
{
    public class CreditacaoFinalStepDomainService_Test
    {
        [Trait("CreditacaoFinalStepDomain", "FinishTheProcessAndSendNotification")]
        [Fact(DisplayName = "Should Finish the process and send a notification to the broker")]
        public void CreditacaoFinalStepDomain_FinishProcess_FinishTheProcessAndSendNotification()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();
            var sagaNotificationMock = new Mock<ISagaNotification>();

            creditacaoDomainMock.Setup(x => x.GetCardAttachments(It.IsAny<string>())).Returns(new List<string>() { "http://google.pt" });
            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(true);
            taskProcessingMock.Setup(x => x.ReturnCardIdFromExternalTask(It.IsAny<ExternalTask>())).Returns(Guid.NewGuid().ToString());
            taskProcessingMock.Setup(x => x.ReturnCourseInstitueFromExternalTask(It.IsAny<ExternalTask>())).Returns("ISMAI");
            taskProcessingMock.Setup(x => x.ReturnCourseNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Informática");
            taskProcessingMock.Setup(x => x.ReturnStudentNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Carlos Campos");
            CreditacaoFinalStepDomainService creditacaoFinalStepDomain = new CreditacaoFinalStepDomainService(creditacaoDomainMock.Object, taskProcessingMock.Object,
                logMock.Object, sagaNotificationMock.Object);
            bool result = creditacaoFinalStepDomain.FinishProcess("ISMAI", new ExternalTask());
            creditacaoDomainMock.Verify(x => x.GetCardAttachments(It.IsAny<string>()), Times.Once);
            taskProcessingMock.Verify(x=>x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.True(result);
        }

        [Trait("CreditacaoFinalStepDomain", "FinishTheProcessAndSendNotification")]
        [Fact(DisplayName = "Should fail to Finish the process")]
        public void CreditacaoFinalStepDomain_FinishProcess_FailToFinishTheProcess()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();
            var sagaNotificationMock = new Mock<ISagaNotification>();

            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(false);
            CreditacaoFinalStepDomainService creditacaoFinalStepDomain = new CreditacaoFinalStepDomainService(creditacaoDomainMock.Object, taskProcessingMock.Object,
                logMock.Object, sagaNotificationMock.Object);
            bool result = creditacaoFinalStepDomain.FinishProcess("ISMAI", new ExternalTask());
            creditacaoDomainMock.Verify(x => x.GetCardAttachments(It.IsAny<string>()), Times.Never);
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.False(result);
        }

        [Trait("CreditacaoFinalStepDomain", "FinishTheProcessAndSendNotification")]
        [Fact(DisplayName = "Should fail because it's summer break time")]
        public void CreditacaoFinalStepDomain_FinishProcess_SummerBreakTime()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();
            var sagaNotificationMock = new Mock<ISagaNotification>();
            creditacaoDomainMock.Setup(x => x.IsSummerBreakTime(It.IsAny<int>())).Returns(true);
            CreditacaoFinalStepDomainService creditacaoFinalStepDomain = new CreditacaoFinalStepDomainService(creditacaoDomainMock.Object, taskProcessingMock.Object,
                logMock.Object, sagaNotificationMock.Object);
            bool result = creditacaoFinalStepDomain.FinishProcess("ISMAI", new ExternalTask());
            Assert.False(result);
        }
    }
}
