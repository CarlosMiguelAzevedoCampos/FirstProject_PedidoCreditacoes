using System.ComponentModel.DataAnnotations;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Models
{
    public class CreditacaoDto
    {
        [Required]
        [Display(Name = "Nome do Aluno")]
        public string StudentName { get; set; }
        [Required]
        [Display(Name = "Nome da Instituição")]
        public string InstituteName { get; set; }
        [Required]
        [Display(Name = "Nome do Curso")]
        public string CourseName { get; set; }
        [Display(Name = "Creditação para CET?")]
        public bool IsCet { get; set; }
        [Required]
        [Display(Name = "Links com documentação necessária.")]
        public string Documents { get; set; }
    }
}
