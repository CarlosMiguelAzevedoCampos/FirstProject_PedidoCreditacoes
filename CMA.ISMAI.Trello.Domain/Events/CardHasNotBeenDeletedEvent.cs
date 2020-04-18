using CMA.ISMAI.Core.Events;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class CardHasNotBeenDeletedEvent : Event
    {
        public CardHasNotBeenDeletedEvent(string cardId)
        {
            CardId = cardId;
        }

        public string CardId { get; set; }
    }
}
