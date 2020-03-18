using CMA.ISMAI.Trello.Domain.Commands;

namespace CMA.ISMAI.Trello.Domain.Validations
{
    public class ObtainCardStatusCommandValidations : CardStatusValidation<CardStatusCommand>
    {
        public ObtainCardStatusCommandValidations()
        {
            ValidateId();
        }
    }
}
