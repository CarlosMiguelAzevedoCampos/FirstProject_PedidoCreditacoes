namespace CMA.ISMAI.Solutions.Creditacoes.UI.Models
{
    internal class Response<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
