using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RideSharing.RideApi.Controllers;
using RideSharing.RideApi.DataAccess;
using RideSharing.Utilities;
using RideSharing.Utilities.Http;

namespace RideSharing.RideMatcher.Tests
{
    public class InMemoryPagedStoredEventHttpClientWrapper : IHttpClientWrapper
    {
        private readonly IList<StoredEventReadModel> _resources;
        private readonly List<HttpRequestMessage> _requestsSent = new List<HttpRequestMessage>();

        public InMemoryPagedStoredEventHttpClientWrapper(
            IReadOnlyList<StoredEventReadModel> resources)
        {
            _resources = resources.ToList();
        }

        public IReadOnlyList<HttpRequestMessage> RequestsSent => _requestsSent;

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            _requestsSent.Add(httpRequestMessage);
            
            var page = PagedResourceUri.ParseRecordRangeFrom(httpRequestMessage.RequestUri);
            var resources = _resources.Where(x => x.Version >= page.Start && x.Version <= page.End);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(resources))
            });
        }
    }
}