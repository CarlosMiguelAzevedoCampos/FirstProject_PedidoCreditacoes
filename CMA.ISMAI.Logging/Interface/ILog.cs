namespace CMA.ISMAI.Logging.Interface
{
    public interface ILog
    {
        void Fatal(string message);
        void Info(string message);
        void Warning(string message);
        void Actions(string message);
    }
}
