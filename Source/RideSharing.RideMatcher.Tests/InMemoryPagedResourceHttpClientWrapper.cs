using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RideSharing.Utilities;

namespace RideSharing.RideMatcher.Tests
{
    public class InMemoryPagedResourceHttpClientWrapper : IHttpClientWrapper
    {
        private readonly IList<object> _resources;
        private readonly List<HttpRequestMessage> _requestsSent = new List<HttpRequestMessage>();

        public InMemoryPagedResourceHttpClientWrapper(
            IReadOnlyList<object> resources)
        {
            _resources = resources.ToList();
        }

        public IReadOnlyList<HttpRequestMessage> RequestsSent => _requestsSent;

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            _requestsSent.Add(httpRequestMessage);
            
            var page = PagedResourceUri.ParseRecordRangeFrom(httpRequestMessage.RequestUri);
            var resources = _resources.Skip(page.Start - 1).Take(page.End - page.Start + 1);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(resources))
            });
        }
    }
}