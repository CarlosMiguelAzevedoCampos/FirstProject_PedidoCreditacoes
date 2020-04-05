namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    internal class ResponseSucess<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
