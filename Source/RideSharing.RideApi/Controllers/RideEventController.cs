using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RideSharing.RideApi.DataAccess;

namespace RideSharing.RideApi.Controllers
{
    [Route("events/[controller]")]
    public class RideEventController : Controller
    {
        [HttpGet]
        public IEnumerable<EventStoreItemReadModel> Get()
        {
            var registrationEventRepository = new RideEventRepository();
            
            return registrationEventRepository.GetAll()
                .Select(x =>
                    new EventStoreItemReadModel
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