using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RideSharing.RideApi;
using RideSharing.RideApi.Controllers;

namespace RideSharing.RideMatcher.Tests
{
    public class FakeRideEventsApi : IRideEventsApi
    {
        private readonly List<StoredEventReadModel> _storedEvents = new List<StoredEventReadModel>();
        
        public FakeRideEventsApi(IEnumerable<StoredEventReadModel> storedEvents)
        {
            _storedEvents.AddRange(storedEvents);
        }
        
        public async Task<IReadOnlyList<StoredEventReadModel>> Get()
        {
            return await Task.FromResult(_storedEvents);
        }
    }
}