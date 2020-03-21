using System;
using System.Collections.Generic;
using System.Text;

namespace CMA.ISMAI.Sagas.Engine.ISMAI.Model
{
    public class CardDto
    {
        public CardDto(string name, DateTime dueTime, string description)
        {
            Name = name;
            DueTime = dueTime;
            Description = description;
        }
        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
    }
}
