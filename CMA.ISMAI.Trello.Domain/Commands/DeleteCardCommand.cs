namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class DeleteCardCommand
    {
        public DeleteCardCommand(string cardId)
        {
            CardId = cardId;
        }

        public string CardId { get; set; }
    }
}
