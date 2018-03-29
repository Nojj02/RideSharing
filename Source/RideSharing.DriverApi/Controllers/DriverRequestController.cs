using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RideSharing.DriverApi.DataAccess;
using RideSharing.DriverApi.Model;

namespace RideSharing.DriverApi.Controllers
{
    [Route("api/[controller]")]
    public class DriverRequestController : Controller
    {
        private readonly DriverRequestRepository _repository = new DriverRequestRepository();

        // GET api/values
        [HttpGet]
        public List<DriverRequestViewModel> Get()
        {
            var requests = _repository.Get();

            return requests.Select(x => new DriverRequestViewModel(x)).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public DriverRequestViewModel Get(Guid id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]DriverRequestPostModel model)
        {
            var driverRequest = new DriverRequest(
                id: Guid.NewGuid(),
                rideId: model.RideId,
                pickupPoint: model.PickupPoint
             );

            await _repository.Save(driverRequest, DateTimeOffset.UtcNow);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
