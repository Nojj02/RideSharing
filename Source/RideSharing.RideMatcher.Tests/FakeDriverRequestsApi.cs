using System.Collections.Generic;
using System.Threading.Tasks;
using RideSharing.DriverApi.Controllers;
using RideSharing.DriverApi.Model;

namespace RideSharing.RideMatcher.Tests
{
    public class FakeDriverRequestsApi : IDriverRequestsApi
    {
        private readonly List<DriverRequestResourceModel> _driverRequestResourceModels = new List<DriverRequestResourceModel>();
        
        public async Task<IReadOnlyList<DriverRequestResourceModel>> Get()
        {
            return await Task.FromResult(_driverRequestResourceModels);
        }

        public async Task Post(DriverRequestPostModel driverRequestPostModel)
        {
            var driverRequest = 
                new DriverRequestResourceModel
                {
                    PickupPoint = driverRequestPostModel.PickupPoint
                };
            _driverRequestResourceModels.Add(driverRequest);
            await Task.CompletedTask;
        }
    }
}