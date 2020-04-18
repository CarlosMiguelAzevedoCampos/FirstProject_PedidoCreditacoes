using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;

namespace CMA.ISMAI.Trello.API.Mapper
{
    public static class Map
    {
        public static AddCardCommand ConvertToAddCardCommand(CardDto card)
        {
            return new AddCardCommand(card.Name, card.DueTime, card.Description, card.BoardId, card.FilesUrl, card.InstituteName, card.CourseName, card.StudentName, card.IsCetOrOtherCondition);
        }

        public static GetCardStatusCommand ConvertToGetCardStatusCommand(string id)
        {
            return new GetCardStatusCommand(id);
        }

        public static GetCardAttachmentsCommand ConvertToGetCardAttachmentsCommand(string id)
        {
            return new GetCardAttachmentsCommand(id);
        }

    }
}
