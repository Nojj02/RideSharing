using System.Net.Http;
using System.Threading.Tasks;

namespace RideSharing.RideMatcher.Tests
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
    }
}