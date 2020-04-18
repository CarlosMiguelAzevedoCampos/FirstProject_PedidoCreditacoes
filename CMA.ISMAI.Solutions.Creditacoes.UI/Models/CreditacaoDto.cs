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
        [Display(Name = "Creditação obtida em CET, experiência Profissional ou outra Formação?")]
        public bool IsCetOrOtherCondition { get; set; }
        [Required]
        [Display(Name = "Links com documentação necessária.")]
        public string Documents { get; set; }
    }
}
