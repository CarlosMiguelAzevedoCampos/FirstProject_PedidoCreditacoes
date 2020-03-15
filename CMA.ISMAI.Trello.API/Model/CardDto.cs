using System;

namespace CMA.ISMAI.Trello.API.Model
{
    public class CardDto
    {
        public CardDto(string name, DateTime dueTime, string description)
        {
            Name = name;
            DueTime = dueTime;
            Description = description;
        }
        protected CardDto()
        {

        }

        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
    }
}
