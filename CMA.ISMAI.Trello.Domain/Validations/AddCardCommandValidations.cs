using CMA.ISMAI.Trello.Domain.Commands;

namespace CMA.ISMAI.Trello.Domain.Validations
{
    public class AddCardCommandValidations : CardValidations<AddCardCommand>
    {
        public AddCardCommandValidations()
        {
            ValidateProcessName();
            ValidateDescription();
            ValidateDueDate();
            ValidateBoardId();
            ValidateFilesUrl();
            ValidateCourseName();
            ValidateInstituteName();
            ValidateStudentName();
        }
    }
}
