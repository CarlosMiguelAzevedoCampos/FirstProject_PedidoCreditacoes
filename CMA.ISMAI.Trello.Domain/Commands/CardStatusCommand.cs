using CMA.ISMAI.Core.Commands;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public abstract class CardStatusCommand : Command
    {
        public string Id { get; set; }
    }
}