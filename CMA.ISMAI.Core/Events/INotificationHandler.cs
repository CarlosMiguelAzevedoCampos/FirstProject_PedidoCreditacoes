namespace CMA.ISMAI.Core.Events
{
    public interface INotificationHandler<T, TO> where T : class where TO : class
    {
        TO Handler(T request);
    }
}
