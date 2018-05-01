using System.Linq;
using System.Threading.Tasks;
using RideSharing.DriverApi.Controllers;
using RideSharing.DriverApi.Model;
using RideSharing.RideApi;
using RideSharing.RideApi.Controllers;
using RideSharing.RideApi.DataAccess;

namespace RideSharing.RideMatcher
{
    public class RequestDriverOnRideRequestedEventHandler
    {
        private readonly IRideEventsApi _rideEventsApi;
        private readonly IDriverRequestsApi _driverRequestsApi;

        public RequestDriverOnRideRequestedEventHandler(IRideEventsApi rideEventsApi, IDriverRequestsApi driverRequestsApi)
        {
            _rideEventsApi = rideEventsApi;
            _driverRequestsApi = driverRequestsApi;
        }

        public async Task Poll()
        {
            var storedEventReadModels = await _rideEventsApi.GetUnprocessedMessages();
            foreach (var storedEventReadModel in 
                storedEventReadModels.Where(x => x.EventType == "RideSharing.RideApi.Model.RideRequestedEvent"))
            {
                RideRequestedEvent rideRequestedEvent = storedEventReadModel.Event;
                
                var driverRequestPostModel = new DriverRequestPostModel
                {
                    PickupPoint = rideRequestedEvent.PickupPoint
                };
                await _driverRequestsApi.Post(driverRequestPostModel);
            }
        }
    }
}