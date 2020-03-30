using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using System.Threading.Tasks;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface
{
    public interface IHttpRequest
    {
        Task<string> PostNewCardAsync(CardDto card);
        Task<bool> CreateNewWorkFlow(DeployDto card);
    }
}
