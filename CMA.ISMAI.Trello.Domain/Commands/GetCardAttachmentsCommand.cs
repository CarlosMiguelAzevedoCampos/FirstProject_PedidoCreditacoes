namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class GetCardAttachmentsCommand
    {
        public GetCardAttachmentsCommand(string cardId)
        {
            CardId = cardId;
        }

        public string CardId { get; set; }
    }
}
