using System;

namespace CMA.ISMAI.Trello.Domain.Model
{
    public class Card
    {
        public Card(string name, DateTime dueTime, string description)
        {
            Name = name;
            DueTime = dueTime;
            Description = description;
        }

        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }

        public bool IsValid(Card card)
        {
            if (string.IsNullOrEmpty(card.Name) || string.IsNullOrEmpty(card.Description) || card.DueTime == null)
                return false;
            return true;
        }
    }
}
