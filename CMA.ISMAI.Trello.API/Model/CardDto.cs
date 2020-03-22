using System;

namespace CMA.ISMAI.Trello.API.Model
{
    public class CardDto
    {
        public CardDto(string name, DateTime dueTime, string description, int boardId)
        {
            Name = name;
            DueTime = dueTime;
            Description = description;
            BoardId = boardId;
        }
        protected CardDto()
        {

        }

        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }
    }
}
