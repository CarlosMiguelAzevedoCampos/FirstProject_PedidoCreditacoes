namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class GetCardAttachmentsCommand
    {
        public GetCardAttachmentsCommand(string cardId, int boardId)
        {
            CardId = cardId;
            BoardId = boardId;
        }

        public string CardId { get; set; }
        public int BoardId { get; set; }
    }
}
