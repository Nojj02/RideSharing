using System.Collections;
using System.Threading.Tasks;
using RideSharing.RideApi.Controllers;
using RideSharing.RideApi.DataAccess;
using Xunit;

namespace RideSharing.RideMatcher.Tests
{
    public class RequestDriverOnRideRequestedEventHandlerTests
    {
        [Fact]
        public async Task Poll_SendSingleDriverRequest_RideHasSingleRideRequest()
        {
            var anEvent =
                new RideRequestedEvent("A nice place", null);
            
            var ride = 
                new StoredEvent
                {
                    EventType = "RideSharing.RideApi.Model.RideRequestedEvent",
                    Event = anEvent
                };
            var rideApi = 
                new FakeRideEventsApi(
                    storedEvents: new[]
                    {
                        new StoredEventReadModel
                        {
                            EventType = ride.EventType,
                            Event = ride.Event
                        }
                    });
            
            var driverRequestsApi = new FakeDriverRequestsApi();
            var eventHandler = new RequestDriverOnRideRequestedEventHandler(rideApi, driverRequestsApi);

            await eventHandler.Poll();

            var driverRequests = await driverRequestsApi.Get();
            Assert.Single(driverRequests);
            Assert.Equal("A nice place", driverRequests[0].PickupPoint);
        }
    }
}