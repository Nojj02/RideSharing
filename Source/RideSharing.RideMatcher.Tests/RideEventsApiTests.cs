using System;
using System.Collections;
using System.Threading.Tasks;
using RideSharing.RideApi.Controllers;
using RideSharing.RideApi.DataAccess;
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
                var httpClientWrapper = 
                    new InMemoryPagedStoredEventHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel(0, "A nice place")
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper, 
                        messageQueueProcessingDetailRepository: new InMemoryMessageQueueProcessingDetailRepository(), 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);

                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Single(httpClientWrapper.RequestsSent);
                Assert.Equal("http://localhost/resource/0,4", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);
                
                Assert.Single(results);
                Assert.Equal("RideSharing.RideApi.Model.RideRequestedEvent", results[0].EventType);
            }
            
            [Fact]
            public async Task ReturnsFirstBatch_NoProcessedEventsYet_MultipleNewEvents()
            {
                var httpClientWrapper = 
                    new InMemoryPagedStoredEventHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel(0, "A nice place"),
                            CreateStoredEventReadModel(1, "A nicer place")
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper, 
                        messageQueueProcessingDetailRepository: new InMemoryMessageQueueProcessingDetailRepository(), 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);
                
                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Single(httpClientWrapper.RequestsSent);
                Assert.Equal("http://localhost/resource/0,4", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);

                Assert.Equal(2, results.Count);
                Assert.Equal("A nice place", (string)results[0].Event.PickupPoint);
                Assert.Equal("A nicer place", (string)results[1].Event.PickupPoint);
            }
            
            [Fact]
            public async Task ReturnsTwoBatches_NoProcessedEventsYet_EventsMoreThanOneBatchSize()
            {
                var httpClientWrapper = 
                    new InMemoryPagedStoredEventHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel(0, "1st"),
                            CreateStoredEventReadModel(1, "2nd"),
                            CreateStoredEventReadModel(2, "3rd"),
                            CreateStoredEventReadModel(3, "4th"),
                            CreateStoredEventReadModel(4, "5th"),
                            CreateStoredEventReadModel(5, "6th"),
                            CreateStoredEventReadModel(6, "7th"),
                            CreateStoredEventReadModel(7, "8th")
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper, 
                        messageQueueProcessingDetailRepository: new InMemoryMessageQueueProcessingDetailRepository(), 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);

                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Equal(2, httpClientWrapper.RequestsSent.Count);
                Assert.Equal("http://localhost/resource/0,4", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);
                Assert.Equal("http://localhost/resource/5,9", httpClientWrapper.RequestsSent[1].RequestUri.AbsoluteUri);

                Assert.Equal(8, results.Count);
                Assert.Equal("1st", (string)results[0].Event.PickupPoint);
                Assert.Equal("8th", (string)results[7].Event.PickupPoint);
            }

            [Fact]
            public async Task ReturnsTwoBatches_NoProcessedEventsYet_EventsExactlyOneBatchSize_StillMakesTwoCalls()
            {
                var httpClientWrapper =
                    new InMemoryPagedStoredEventHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel(0, "1st"),
                            CreateStoredEventReadModel(1, "2nd"),
                            CreateStoredEventReadModel(2, "3rd"),
                            CreateStoredEventReadModel(3, "4th"),
                            CreateStoredEventReadModel(4, "5th")
                        });

                var rideEventsApi =
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper,
                        messageQueueProcessingDetailRepository: new InMemoryMessageQueueProcessingDetailRepository(), 
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize: 5);

                var results = await rideEventsApi.GetUnprocessedMessages();

                Assert.Equal(2, httpClientWrapper.RequestsSent.Count);
                Assert.Equal("http://localhost/resource/0,4", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);
                Assert.Equal("http://localhost/resource/5,9", httpClientWrapper.RequestsSent[1].RequestUri.AbsoluteUri);

                Assert.Equal(5, results.Count);
                Assert.Equal("1st", (string)results[0].Event.PickupPoint);
                Assert.Equal("5th", (string)results[4].Event.PickupPoint);
            }
            
            [Fact]
            public async Task SkipsPagesWithProcessedMessages_HasProcessedMessages_MultiplePagesSkipped()
            {
                var messageQueueProcessingDetailRepository =
                    new InMemoryMessageQueueProcessingDetailRepository(
                        new[] {
                            new MessageQueueProcessingDetail(
                                queueName: "ride",
                                lastMessageNumber: 7
                            )
                        });
                
                var httpClientWrapper = 
                    new InMemoryPagedStoredEventHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel(0, "1st"),
                            CreateStoredEventReadModel(1, "2nd"),
                            CreateStoredEventReadModel(2, "3rd"),
                            CreateStoredEventReadModel(3, "4th"),
                            CreateStoredEventReadModel(4, "5th"),
                            CreateStoredEventReadModel(5, "6th"),
                            CreateStoredEventReadModel(6, "7th"),
                            CreateStoredEventReadModel(7, "8th"),
                            CreateStoredEventReadModel(8, "9th"),
                            CreateStoredEventReadModel(9, "10th"),
                            CreateStoredEventReadModel(10, "11th")
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper,
                        messageQueueProcessingDetailRepository: messageQueueProcessingDetailRepository,
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 3);

                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Equal(2, httpClientWrapper.RequestsSent.Count);
                Assert.Equal("http://localhost/resource/6,8", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);
                Assert.Equal("http://localhost/resource/9,11", httpClientWrapper.RequestsSent[1].RequestUri.AbsoluteUri);

                Assert.Equal(3, results.Count);
                Assert.Equal("9th", (string)results[0].Event.PickupPoint);
                Assert.Equal("10th", (string)results[1].Event.PickupPoint);
                Assert.Equal("11th", (string)results[2].Event.PickupPoint);
            }
            
            [Fact]
            public async Task SkipsPagesWithProcessedMessages_HasProcessedMessages()
            {
                var messageQueueProcessingDetailRepository =
                    new InMemoryMessageQueueProcessingDetailRepository(
                        new[] {
                            new MessageQueueProcessingDetail(
                                queueName: "ride",
                                lastMessageNumber: 5
                            )
                        });
                
                var httpClientWrapper = 
                    new InMemoryPagedStoredEventHttpClientWrapper(
                        resources: new[]
                        {
                            CreateStoredEventReadModel(0, "1st"),
                            CreateStoredEventReadModel(1, "2nd"),
                            CreateStoredEventReadModel(2, "3rd"),
                            CreateStoredEventReadModel(3, "4th"),
                            CreateStoredEventReadModel(4, "5th"),
                            CreateStoredEventReadModel(5, "6th"),
                            CreateStoredEventReadModel(6, "7th"),
                            CreateStoredEventReadModel(7, "8th"),
                            CreateStoredEventReadModel(8, "9th")
                        });
                
                var rideEventsApi = 
                    new RideEventsApi(
                        httpClientWrapper: httpClientWrapper,
                        messageQueueProcessingDetailRepository: messageQueueProcessingDetailRepository,
                        baseUri: new Uri("http://localhost/resource/"),
                        pageSize : 5);

                var results = await rideEventsApi.GetUnprocessedMessages();
                
                Assert.Equal(1, httpClientWrapper.RequestsSent.Count);
                Assert.Equal("http://localhost/resource/5,9", httpClientWrapper.RequestsSent[0].RequestUri.AbsoluteUri);

                Assert.Equal(3, results.Count);
                Assert.Equal("7th", (string)results[0].Event.PickupPoint);
                Assert.Equal("8th", (string)results[1].Event.PickupPoint);
                Assert.Equal("9th", (string)results[2].Event.PickupPoint);
            }

            private static StoredEventReadModel CreateStoredEventReadModel(int version, string pickupPoint)
            {
                var anEvent =
                    new RideRequestedEvent(pickupPoint, null)
                    {
                        Version = version
                    };

                var storedEvent =
                    new StoredEvent
                    {
                        Id = anEvent.Id,
                        Version = anEvent.Version,
                        EventType = "RideSharing.RideApi.Model.RideRequestedEvent",
                        Event = anEvent,
                        TimeStamp = DateTimeOffset.UtcNow
                    };

                var storedEventReadModel = new StoredEventReadModel(storedEvent);
                return storedEventReadModel;
            }
        }
    }
}