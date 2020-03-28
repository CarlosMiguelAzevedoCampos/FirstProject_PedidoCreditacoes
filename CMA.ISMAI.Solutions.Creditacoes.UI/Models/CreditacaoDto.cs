using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Models
{
    public class CreditacaoDto
    {
        public string StudentName { get; set; }
        public string InstituteName { get; set; }
        public string CourseName { get; set; }
        public bool IsCet { get; set; }
        public List<IFormFile> Attachments { get; set; } 
    }
}
