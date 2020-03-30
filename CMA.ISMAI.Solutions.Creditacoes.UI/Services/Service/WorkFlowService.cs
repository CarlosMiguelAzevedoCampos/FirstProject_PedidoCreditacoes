using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service
{
    public class WorkFlowService : IWorkFlowService
    {
        private readonly IHttpRequest _httpRequest;
        public WorkFlowService(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public bool CreateWorkFlowProcess(CreditacaoDto creditacaoDto, string cardId)
        {
            var deploy = new DeployDto("ISMAI", new Dictionary<string, object>()
            {
                {"cet" ,creditacaoDto.IsCet },
                {"cardId",cardId },
                {"courseName",creditacaoDto.CourseName },
                {"studentName",creditacaoDto.StudentName },
                {"courseInstitute",creditacaoDto.InstituteName }
            });
            var result = _httpRequest.CreateNewWorkFlow(deploy);
            return result.Result;
        }
    }
}
