using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RideSharing.RideApi.DataAccess;
using RideSharing.RideApi.Model;

namespace RideSharing.RideApi.Controllers
{
    [Route("api/[controller]")]
    public class RidesController : Controller
    {
        // GET api/rides
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/rides/5
        [HttpGet("{id}")]
        public RideViewModel Get(Guid id)
        {
            var rideRepository = new RideRepository();
            var ride = rideRepository.Get(id);
            
            var rideViewModel = new RideViewModel(ride.Id, ride.PickupPoint, ride.Destination, ride.State);
            return rideViewModel;
        }

        // POST api/rides
        [HttpPost]
        public async Task<RideViewModel> Post([FromBody]RidePostModel ridePostModel)
        {
            var requestGuid = Guid.NewGuid(); 
            var ride = new Ride(requestGuid, ridePostModel.PickupPoint, ridePostModel.Destination);

            var rideRepository = new RideRepository();
            await rideRepository.Save(ride, DateTimeOffset.UtcNow);

            var rideViewModel = new RideViewModel(requestGuid, ridePostModel.PickupPoint, ridePostModel.Destination, ride.State);
            return rideViewModel;
        }

        // POST api/rides/5/accept
        [HttpPost("{id}/accept")]
        public async Task<RideViewModel> Accept(Guid id)
        { 
            var rideRepository = new RideRepository();
            var ride = rideRepository.Get(id);
            ride.Accept();

            await rideRepository.Update(ride, DateTimeOffset.UtcNow);

            var rideViewModel = new RideViewModel(id, ride.PickupPoint, ride.Destination, ride.State);
            return rideViewModel;
        }

        // PUT api/rides/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/rides/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
