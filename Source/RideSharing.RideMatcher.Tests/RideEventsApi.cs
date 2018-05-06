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
        private const string QueueName = "ride";
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IMessageQueueProcessingDetailRepository _messageQueueProcessingDetailRepository;
        private readonly Uri _baseUri;
        private readonly int _pageSize;

        public RideEventsApi(
            IHttpClientWrapper httpClientWrapper,
            IMessageQueueProcessingDetailRepository messageQueueProcessingDetailRepository,
            Uri baseUri,
            int pageSize)
        {
            _httpClientWrapper = httpClientWrapper;
            _messageQueueProcessingDetailRepository = messageQueueProcessingDetailRepository;
            _baseUri = baseUri;
            _pageSize = pageSize;
        }

        public async Task<IReadOnlyList<StoredEventReadModel>> GetUnprocessedMessages()
        {
            var messageQueueProcessingDetail = _messageQueueProcessingDetailRepository.Get(QueueName);
            var lastProcessedMessageNumber = messageQueueProcessingDetail != null
                ? messageQueueProcessingDetail.LastMessageNumber
                : -1;
            
            var storedEventReadModels = new List<StoredEventReadModel>();

            var pagesToSkip = lastProcessedMessageNumber / _pageSize;
            var rangeStart = _pageSize * pagesToSkip;
            var inRangeStoredEventReadModels = await GetStoredEventReadModels(rangeStart);
            var newStoredEventReadModels = 
                inRangeStoredEventReadModels
                    .Where(x => x.Version > lastProcessedMessageNumber).ToList();
            storedEventReadModels.AddRange(newStoredEventReadModels);

            while (inRangeStoredEventReadModels.Count >= _pageSize)
            {
                rangeStart += _pageSize;
                inRangeStoredEventReadModels = await GetStoredEventReadModels(rangeStart);
                storedEventReadModels.AddRange(inRangeStoredEventReadModels);
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