using System;
using RideSharing.RideApi.DataAccess;

namespace RideSharing.RideApi.Controllers
{
    public class StoredEventReadModel
    {
        public StoredEventReadModel()
        {
        }

        public StoredEventReadModel(StoredEvent storedEvent)
        {
            Id = storedEvent.Id;
            Version = storedEvent.Version;
            EventType = storedEvent.EventType;
            Event = storedEvent.Event;
            TimeStamp = storedEvent.TimeStamp;
        }
        
        public Guid Id { get; set; }
        
        public string EventType { get; set; }
        
        public int Version { get; set; }
        
        public dynamic Event { get; set; }
        
        public DateTimeOffset TimeStamp { get; set; }
    }
}