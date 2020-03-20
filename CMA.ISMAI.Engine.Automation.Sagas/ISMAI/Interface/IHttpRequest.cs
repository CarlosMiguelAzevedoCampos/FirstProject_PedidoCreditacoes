using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface
{
    public interface IHttpRequest
    {
        Task<bool> CardStateAsync(string cardId);
    }
}
