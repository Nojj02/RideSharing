using System.Net.Http;
using System.Threading.Tasks;

namespace RideSharing.RideMatcher
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
    }
}