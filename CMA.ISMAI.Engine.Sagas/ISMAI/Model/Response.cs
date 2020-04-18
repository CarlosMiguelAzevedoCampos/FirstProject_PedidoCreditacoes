namespace CMA.ISMAI.Sagas.Service.Model
{
    internal class Response<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
