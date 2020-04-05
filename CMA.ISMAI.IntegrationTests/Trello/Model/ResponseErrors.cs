namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    internal class ResponseErrors<T> where T : class
    {
        public bool Success { get; set; }
        public T Errors { get; set; }
    }
}
