using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RideSharing.RideApi.Controllers;
using RideSharing.RideApi.DataAccess;
using RideSharing.Utilities;
using Xunit;

namespace RideSharing.RideMatcher.Tests
{
    public class RideEventsApiTests
    {
        public class GetUnprocessedMessagesTests
        {
            [Fact]
            public async Task ReturnsFirstBatch_NoProcessedEventsYet()
            {
                var anEvent =
                    new RideRequestedEvent("A nice place", null);
            
                var ride = 
                    new StoredEvent
                    {
                        EventType = "RideSharing.RideApi.Model.RideRequestedEvent",
                        Event = anEvent
                    };
                
                var httpClientWrapper = 
                    new PagedResourceHttpClientWrapper(
                        resources: new[]
                        {
                            new StoredEventReadModel
                            {
                                EventType = ride.EventType,
                                Event = ride.Event
                            }
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper, 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);

                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Single(httpClientWrapper.RequestsSent);
                Assert.Equal("http://localhost/resource/1,5", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);
                
                Assert.Single(results);
                Assert.Equal("RideSharing.RideApi.Model.RideRequestedEvent", results[0].EventType);
            }
            
            [Fact]
            public async Task ReturnsFirstBatch_NoProcessedEventsYet_MultipleNewEvents()
            {
                var anEvent =
                    new RideRequestedEvent("A nice place", null);
            
                var ride = 
                    new StoredEvent
                    {
                        EventType = "RideSharing.RideApi.Model.RideRequestedEvent",
                        Event = anEvent
                    };
                
                var anotherEvent =
                    new RideRequestedEvent("A nicer place", null);
            
                var anotherRide = 
                    new StoredEvent
                    {
                        EventType = "RideSharing.RideApi.Model.RideRequestedEvent",
                        Event = anotherEvent
                    };
                
                var httpClientWrapper = 
                    new PagedResourceHttpClientWrapper(
                        resources: new[]
                        {
                            new StoredEventReadModel
                            {
                                EventType = ride.EventType,
                                Event = ride.Event
                            },
                            new StoredEventReadModel
                            {
                                EventType = anotherRide.EventType,
                                Event = anotherRide.Event
                            }
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper, 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);
                
                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Single(httpClientWrapper.RequestsSent);
                Assert.Equal("http://localhost/resource/1,5", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);

                Assert.Equal(2, results.Count);
                Assert.Equal("A nice place", (string)results[0].Event.PickupPoint);
                Assert.Equal("A nicer place", (string)results[1].Event.PickupPoint);
            }
            
            [Fact]
            public async Task ReturnsTwoBatches_NoProcessedEventsYet_EventsMoreThanOneBatchSize()
            {
                var httpClientWrapper = 
                    new PagedResourceHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel("1st"),
                            CreateStoredEventReadModel("2nd"),
                            CreateStoredEventReadModel("3rd"),
                            CreateStoredEventReadModel("4th"),
                            CreateStoredEventReadModel("5th"),
                            CreateStoredEventReadModel("6th"),
                            CreateStoredEventReadModel("7th"),
                            CreateStoredEventReadModel("8th"),
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper, 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);

                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Equal(2, httpClientWrapper.RequestsSent.Count);
                Assert.Equal("http://localhost/resource/1,5", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);
                Assert.Equal("http://localhost/resource/6,10", httpClientWrapper.RequestsSent[1].RequestUri.AbsoluteUri);

                Assert.Equal(8, results.Count);
                Assert.Equal("1st", (string)results[0].Event.PickupPoint);
                Assert.Equal("8th", (string)results[7].Event.PickupPoint);
            }

            private static StoredEventReadModel CreateStoredEventReadModel(string pickupPoint)
            {
                var anEvent =
                    new RideRequestedEvent(pickupPoint, null);

                var ride =
                    new StoredEvent
                    {
                        EventType = "RideSharing.RideApi.Model.RideRequestedEvent",
                        Event = anEvent
                    };

                var storedEventReadModel = new StoredEventReadModel
                {
                    EventType = ride.EventType,
                    Event = ride.Event
                };
                return storedEventReadModel;
            }
        }
    }

    public class PagedResourceHttpClientWrapper : IHttpClientWrapper
    {
        private readonly IList<object> _resources;
        private readonly List<HttpRequestMessage> _requestsSent = new List<HttpRequestMessage>();

        public PagedResourceHttpClientWrapper(
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

    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
    }
}