using System;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface
{
    public interface IHttpRequest
    {
        Task<bool> CardStateAsync(string cardId);
        Task<string> CardPostAsync(string name, DateTime dueTime, string description);
    }
}
