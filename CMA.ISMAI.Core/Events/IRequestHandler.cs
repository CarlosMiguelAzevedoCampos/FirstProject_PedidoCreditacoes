namespace CMA.ISMAI.Core.Events
{
    public interface IRequestHandler<T, TO> where T : class where TO : class
    {
        TO Handler(T request);
    }
}
