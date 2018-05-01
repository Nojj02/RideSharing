using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RideSharing.RideApi;
using RideSharing.RideApi.Controllers;

namespace RideSharing.RideMatcher.Tests
{
    public class RideEventsApi : IRideEventsApi
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly Uri _baseUri;
        private readonly int _pageSize;

        public RideEventsApi(
            IHttpClientWrapper httpClientWrapper, 
            Uri baseUri,
            int pageSize)
        {
            _httpClientWrapper = httpClientWrapper;
            _baseUri = baseUri;
            _pageSize = pageSize;
        }

        public async Task<IReadOnlyList<StoredEventReadModel>> GetUnprocessedMessages()
        {
            var storedEventReadModels = new List<StoredEventReadModel>();

            var rangeStart = 1;
            var newStoredEventReadModels = await GetStoredEventReadModels(rangeStart);
            storedEventReadModels.AddRange(newStoredEventReadModels);

            while (newStoredEventReadModels.Count >= _pageSize)
            {
                rangeStart += _pageSize;
                newStoredEventReadModels = await GetStoredEventReadModels(rangeStart);
                storedEventReadModels.AddRange(newStoredEventReadModels);
            }
            
            return storedEventReadModels;
        }

        private async Task<List<StoredEventReadModel>> GetStoredEventReadModels(int rangeStart)
        {
            var uri = new Uri(_baseUri, $"{rangeStart},{rangeStart + _pageSize - 1}");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClientWrapper.SendAsync(requestMessage);
            var contentAsString = await response.Content.ReadAsStringAsync();
            var newStoredEventReadModels =
                JsonConvert.DeserializeObject<IEnumerable<StoredEventReadModel>>(contentAsString).ToList();
            return newStoredEventReadModels;
        }
    }
}