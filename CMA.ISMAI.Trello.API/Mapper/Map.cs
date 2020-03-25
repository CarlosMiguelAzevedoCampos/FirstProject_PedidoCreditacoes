using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;

namespace CMA.ISMAI.Trello.API.Mapper
{
    public static class Map
    {
        public static AddCardCommand ConverToModel(CardDto card)
        {
            return new AddCardCommand(card.Name, card.DueTime, card.Description, card.BoardId, card.FilesUrl);
        }
    }
}
