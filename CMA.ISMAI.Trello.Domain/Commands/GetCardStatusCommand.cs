namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class GetCardStatusCommand
    {
        public GetCardStatusCommand(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }
    }
}
