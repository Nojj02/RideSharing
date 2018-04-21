using System.Collections.Generic;
using System.Threading.Tasks;
using RideSharing.RideApi.Controllers;

namespace RideSharing.RideMatcher.Tests
{
    public class FakeRideApi : IRideApi
    {
        private readonly List<RideResourceModel> _rideResourceModels = new List<RideResourceModel>();
        
        public async Task Post(RidePostModel ride)
        {
            var rideResourceModel =
                new RideResourceModel
                {
                    PickupPoint = ride.PickupPoint
                };
            _rideResourceModels.Add(rideResourceModel);
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyList<RideResourceModel>> Get()
        {
            return await Task.FromResult(_rideResourceModels);
        }
    }
}