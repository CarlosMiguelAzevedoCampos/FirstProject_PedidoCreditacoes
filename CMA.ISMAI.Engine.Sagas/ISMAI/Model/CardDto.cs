using System;
using System.Collections.Generic;
using System.Text;

namespace CMA.ISMAI.Sagas.Engine.ISMAI.Model
{
    public class CardDto
    {
        public CardDto(string name, DateTime dueTime, int boardId, string description)
        {
            Name = name;
            DueTime = dueTime;
            Description = description;
            BoardId = boardId;
        }
        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public int BoardId { get; set; }
        public string Description { get; set; }
    }
}
