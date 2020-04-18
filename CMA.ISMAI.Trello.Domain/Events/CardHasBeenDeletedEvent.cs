using CMA.ISMAI.Core.Events;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class CardHasBeenDeletedEvent : Event
    {
        public CardHasBeenDeletedEvent(string cardId)
        {
            CardId = cardId;
        }

        public string CardId { get; set; }
    }
}
