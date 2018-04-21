using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RideSharing.RideApi.DataAccess;

namespace RideSharing.RideApi.Controllers.EventControllers
{
    [Route("events/[controller]")]
    public class RideEventController : Controller
    {
        [HttpGet]
        public IEnumerable<StoredItemReadModel> Get()
        {
            var registrationEventRepository = new RideEventRepository();
            
            return registrationEventRepository.GetAll()
                .Select(x =>
                    new StoredItemReadModel
                    {
                        Id = x.Id,
                        EventType = x.EventType,
                        Event = x.Event,
                        TimeStamp = x.TimeStamp
                    })
                .ToList();
        }
    }
}