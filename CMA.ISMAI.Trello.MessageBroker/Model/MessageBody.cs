namespace CMA.ISMAI.Trello.MessageBroker.Model
{
    public class MessageBody
    {
        public MessageBody(string to, string message)
        {
            To = to;
            Message = message;
        }

        public string To { get; set; }
        public string Message { get; set; }
    }
}
