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
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .NotNull().WithMessage("Please ensure you have entered the Name");
        }

        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                 .NotEmpty().WithMessage("Please ensure you have entered the Description")
                .NotNull().WithMessage("Please ensure you have entered the Description");
        }

        protected void ValidateDueDate()
        {
            RuleFor(c => c.DueTime)
                .NotEmpty()
                .Must(TimeToFinishIt)
                .WithMessage("The card must have an date bigger than the today date!");
        }

        protected void ValidateBoardId()
        {
            RuleFor(c => c.BoardId)
                 .GreaterThanOrEqualTo(0).WithMessage("The card must have an boardId bigger or equal than the 0!");
        }

        protected void ValidateFilesUrl()
        {
            RuleFor(c => c.FilesUrl)
                .NotNull().WithMessage("Please ensure you have entered the files url list");
        }

        private bool TimeToFinishIt(DateTime arg)
        {
            return arg > DateTime.Now;
        }
    }
}
