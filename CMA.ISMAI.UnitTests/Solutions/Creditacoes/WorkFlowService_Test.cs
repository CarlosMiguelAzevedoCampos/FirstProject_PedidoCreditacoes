using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Solutions.Creditacoes
{
    public class WorkFlowService_Test
    {
        [Fact]
        private void WorkFlowService_StartWorkFlow_DeployAndStartWorkFlow()
        {
            var httpMock = new Mock<IHttpRequest>();
            httpMock.Setup(x => x.CreateNewWorkFlow(It.IsAny<DeployDto>())).Returns(Task.FromResult(true));
            IWorkFlowService workFlowService = new WorkFlowService(httpMock.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            bool result = workFlowService.CreateWorkFlowProcess(creditacaoDto, Guid.NewGuid().ToString());
            Assert.True(result);
        }

        [Fact]
        private void WorkFlowService_StartWorkFlow_FailOnDeploy()
        {
            var httpMock = new Mock<IHttpRequest>();
            httpMock.Setup(x => x.CreateNewWorkFlow(It.IsAny<DeployDto>())).Returns(Task.FromResult(false));
            IWorkFlowService workFlowService = new WorkFlowService(httpMock.Object);
            var creditacaoDto = new CreditacaoDto() { CourseName = "Informática", IsCet = false, Documents = "", InstituteName = "ISMAI", StudentName = "Carlos Campos" };
            bool result = workFlowService.CreateWorkFlowProcess(creditacaoDto, Guid.NewGuid().ToString());
            Assert.False(result);
        }
    }
}
