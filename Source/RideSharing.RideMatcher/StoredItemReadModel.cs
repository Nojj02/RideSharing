using System;

namespace RideSharing.RideMatcher
{
    public class StoredItemReadModel
    {
        public Guid Id { get; set; }
        
        public string EventType { get; set; }
        
        public dynamic Event { get; set; }
        
        public DateTimeOffset TimeStamp { get; set; }
    }
}