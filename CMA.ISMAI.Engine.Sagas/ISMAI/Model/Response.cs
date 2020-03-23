namespace CMA.ISMAI.Sagas.Engine.ISMAI.Model
{
    internal class Response
    {
        public bool Success { get; set; }
        public AddCardCompletedEvent Data { get; set; }
    }
}
