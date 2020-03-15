using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Model;

namespace CMA.ISMAI.Trello.API.Mapper
{
    public static class Map
    {
        public static Card ConverToModel(CardDto card)
        {
            return new Card(card.Name, card.DueTime, card.Description);
        }
    }
}
