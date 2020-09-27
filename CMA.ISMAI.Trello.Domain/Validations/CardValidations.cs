using CMA.ISMAI.Trello.Domain.Commands;
using FluentValidation;
using System;

namespace CMA.ISMAI.Trello.Domain.Validations
{
    public abstract class CardValidations<T> : AbstractValidator<T> where T : CardCommand
    {
        protected void ValidateProcessName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Porfavor insira um nome para o Processo!")
                .NotNull().WithMessage("Porfavor insira um nome para o Processo!");
        }

        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                 .NotEmpty().WithMessage("Porfavor, verifique que colocou uma descrição")
                .NotNull().WithMessage("Porfavor insira uma descrição!");
        }

        protected void ValidateDueDate()
        {
            RuleFor(c => c.DueTime)
                .NotEmpty()
                .Must(TimeToFinishIt)
                .WithMessage("A carta tem de ter uma data de fim maior que a de hoje!");
        }

        protected void ValidateBoardId()
        {
            RuleFor(c => c.BoardId)
                 .GreaterThanOrEqualTo(0).WithMessage("A carta tem de ter um ID maior ou igual a 0!");
        }

        protected void ValidateFilesUrl()
        {
            RuleFor(c => c.FilesUrl)
                .NotNull().WithMessage("Porfavor, verifique que inseriu os links para o ficheiros");
        }

        protected void ValidateInstituteName()
        {
            RuleFor(c => c.InstituteName)
                                .NotEmpty().WithMessage("Porfavor insira o nome da Instituição!")
                .NotNull().WithMessage("Porfavor insira o nome da Instituição!");
        }
        protected void ValidateCourseName()
        {
            RuleFor(c => c.CourseName)
                .NotNull().WithMessage("Porfavor insira o nome do curso!").
                    NotEmpty().WithMessage("Porfavor insira o nome do curso");
        }
        protected void ValidateStudentName()
        {
            RuleFor(c => c.StudentName)
                .NotNull().WithMessage("Porfavor, insira um nome de um Aluno").
                    NotEmpty().WithMessage("Porfavor, insira um nome de um Aluno");
        }

        private bool TimeToFinishIt(DateTime arg)
        {
            return arg > DateTime.Now;
        }
    }
}
