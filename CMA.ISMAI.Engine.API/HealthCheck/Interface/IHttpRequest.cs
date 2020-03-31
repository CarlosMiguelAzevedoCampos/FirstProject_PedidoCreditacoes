using System.Net.Http;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.API.HealthCheck.Interface
{
    public interface IHttpRequest
    {
        Task<HttpResponseMessage> MakeAnHttpRequest(string url);
    }
}
