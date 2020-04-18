using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service;
using Moq;
using System;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Sagas
{
    public class CreditacaoWithCardCreationDomain_Test
    {
        [Trait("CreditacaoWithCardCreationDomain", "CreateCardAndFinishProcess")]
        [Fact(DisplayName = "Should finish the process")]
        public void CreditacaoWithCardCreationDomain_CreateCardAndFinishProcess_ShouldFinishTheProcess()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();

            creditacaoDomainMock.Setup(x => x.CreateNewCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                 It.IsAny<DateTime>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(Guid.NewGuid().ToString());

            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(true);

            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");

            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0,DateTime.Now.AddDays(1), true);

            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(4));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.True(result);
        }

        [Trait("CreditacaoWithCardCreationDomain", "CreateCardAndFinishProcess")]
        [Fact(DisplayName = "Should fail to finish the process")]
        public void CreditacaoWithCardCreationDomain_CreateCardAndFinishProcess_FailToFinshTheProcess()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();

            creditacaoDomainMock.Setup(x => x.CreateNewCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                 It.IsAny<DateTime>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(Guid.NewGuid().ToString());

            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(false);

            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");

            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0, DateTime.Now.AddDays(1), true);

            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(4));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.False(result);
        }

        [Trait("CreditacaoWithCardCreationDomain", "CreateCardAndFinishProcess")]
        [Fact(DisplayName = "Should fail to create a new card")]
        public void CreditacaoWithCardCreationDomain_CreateCardAndFinishProcess_FailToCreateTheNewCard()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();

            creditacaoDomainMock.Setup(x => x.CreateNewCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                 It.IsAny<DateTime>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(string.Empty);

            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");

            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0, DateTime.Now.AddDays(1), true);

            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(4));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Never);
            Assert.False(result);
        }
    }
}
