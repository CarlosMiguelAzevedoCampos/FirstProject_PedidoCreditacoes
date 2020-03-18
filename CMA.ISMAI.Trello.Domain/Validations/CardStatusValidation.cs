using CMA.ISMAI.Trello.Domain.Commands;
using FluentValidation;

namespace CMA.ISMAI.Trello.Domain.Validations
{
    public abstract class CardStatusValidation<T> : AbstractValidator<T> where T : CardStatusCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                 .NotEmpty().WithMessage("Please ensure you have entered the Id")
                .NotNull().WithMessage("Please ensure you have entered the Id");
        }
    }
}
