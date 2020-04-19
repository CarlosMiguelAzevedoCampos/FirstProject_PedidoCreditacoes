using CamundaClient.Dto;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service.Creditacao;
using Moq;
using System;
using Xunit;

namespace CMA.ISMAI.UnitTests.Sagas.Domain
{
    public class CreditacaoWithCardCreationDomainService_Test
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

            taskProcessingMock.Setup(x => x.ReturnCardIdFromExternalTask(It.IsAny<ExternalTask>())).Returns(Guid.NewGuid().ToString());
            taskProcessingMock.Setup(x => x.ReturnCourseInstitueFromExternalTask(It.IsAny<ExternalTask>())).Returns("ISMAI");
            taskProcessingMock.Setup(x => x.ReturnCourseNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Informática");
            taskProcessingMock.Setup(x => x.ReturnStudentNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Carlos Campos");

            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0,DateTime.Now.AddDays(1), true);

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

            taskProcessingMock.Setup(x => x.ReturnCardIdFromExternalTask(It.IsAny<ExternalTask>())).Returns(Guid.NewGuid().ToString());
            taskProcessingMock.Setup(x => x.ReturnCourseInstitueFromExternalTask(It.IsAny<ExternalTask>())).Returns("ISMAI");
            taskProcessingMock.Setup(x => x.ReturnCourseNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Informática");
            taskProcessingMock.Setup(x => x.ReturnStudentNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Carlos Campos");

            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0, DateTime.Now.AddDays(1), true);

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

            taskProcessingMock.Setup(x => x.ReturnCardIdFromExternalTask(It.IsAny<ExternalTask>())).Returns(Guid.NewGuid().ToString());
            taskProcessingMock.Setup(x => x.ReturnCourseInstitueFromExternalTask(It.IsAny<ExternalTask>())).Returns("ISMAI");
            taskProcessingMock.Setup(x => x.ReturnCourseNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Informática");
            taskProcessingMock.Setup(x => x.ReturnStudentNameFromExternalTask(It.IsAny<ExternalTask>())).Returns("Carlos Campos");

            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0, DateTime.Now.AddDays(1), true);

            taskProcessingMock.Verify(x => x.FinishTasks(It.IsAny<string>(), It.IsAny<string>(), null), Times.Never);
            Assert.False(result);
        }

        [Trait("CreditacaoWithCardCreationDomain", "CreateCardAndFinishProcess")]
        [Fact(DisplayName = "Should fail because it's summer break time")]
        public void CreditacaoWithCardCreationDomain_FinishProcess_SummerBreakTime()
        {
            var logMock = new Mock<ILog>();
            var creditacaoDomainMock = new Mock<ICreditacaoDomainService>();
            var taskProcessingMock = new Mock<ITaskProcessingDomainService>();
            creditacaoDomainMock.Setup(x => x.IsSummerBreakTime(It.IsAny<int>())).Returns(true);
            CreditacaoWithCardCreationDomainService creditacaoWithCardCreationDomain = new CreditacaoWithCardCreationDomainService(creditacaoDomainMock.Object, logMock.Object,
                  taskProcessingMock.Object);
            bool result = creditacaoWithCardCreationDomain.CreateCardAndFinishProcess("ISMAI", new ExternalTask(), 0, DateTime.Now.AddDays(1), true);
            Assert.False(result);
        }
    }
}
