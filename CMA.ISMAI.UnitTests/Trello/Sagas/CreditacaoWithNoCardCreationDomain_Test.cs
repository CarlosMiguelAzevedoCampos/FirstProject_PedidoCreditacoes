using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service;
using Moq;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Sagas
{
    public class CreditacaoWithNoCardCreationDomain_Test
    {
        [Trait("CreditacaoWithNoCardCreationDomain", "ValidCardStateAndFinishProcess")]
        [Fact(DisplayName = "Should finish the process")]
        public void CreditacaoWithNoCardCreationDomain_ValidCardStateAndFinishProcess_FinishTheProcess()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();

            creditacaoDomainMock.Setup(x => x.GetCardStatus(It.IsAny<string>())).Returns(true);

            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(true);

            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");

            CreditacaoWithNoCardCreationService creditacaoWithNoCardCreation = new CreditacaoWithNoCardCreationService(logMock.Object, creditacaoDomainMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("ISMAI", new ExternalTask());

            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(4));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.True(result);
        }

        [Trait("CreditacaoWithNoCardCreationDomain", "ValidCardStateAndFinishProcess")]
        [Fact(DisplayName = "Card isn't finished")]
        public void CreditacaoWithNoCardCreationDomain_ValidCardStateAndFinishProcess_CardIsntFinished()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();

            creditacaoDomainMock.Setup(x => x.GetCardStatus(It.IsAny<string>())).Returns(false);
            
            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");

            CreditacaoWithNoCardCreationService creditacaoWithNoCardCreation = new CreditacaoWithNoCardCreationService(logMock.Object, creditacaoDomainMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("ISMAI", new ExternalTask());

            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(4));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Never);
            Assert.False(result);
        }

        [Trait("CreditacaoWithNoCardCreationDomain", "ValidCardStateAndFinishProcess")]
        [Fact(DisplayName = "Process failed to complete")]
        public void CreditacaoWithNoCardCreationDomain_ValidCardStateAndFinishProcess_FailToFinishTheProcess()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();

            creditacaoDomainMock.Setup(x => x.GetCardStatus(It.IsAny<string>())).Returns(true);

            taskProcessingMock.Setup(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null
                )).Returns(false);

            taskProcessingMock.Setup(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>())).Returns("Carlos Campos");

            CreditacaoWithNoCardCreationService creditacaoWithNoCardCreation = new CreditacaoWithNoCardCreationService(logMock.Object, creditacaoDomainMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithNoCardCreation.ValidCardStateAndFinishProcess("ISMAI", new ExternalTask());

            taskProcessingMock.Verify(x => x.ReturnValueFromExternalTask(It.IsAny<ExternalTask>(), It.IsAny<string>()), Times.Exactly(4));
            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            Assert.False(result);
        }
    }
}
