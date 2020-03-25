using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.API.Model
{
    public class CardDto
    {
        public CardDto(string name, DateTime dueTime, string description, int boardId, List<string> filesUrl)
        {
            Name = name;
            DueTime = dueTime;
            Description = description;
            BoardId = boardId;
            FilesUrl = filesUrl;
        }
        protected CardDto()
        {

        }

        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }
        public List<string> FilesUrl { get; set; }
    }
}
