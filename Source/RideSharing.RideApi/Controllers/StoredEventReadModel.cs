﻿using System;

namespace RideSharing.RideApi.Controllers
{
    public class StoredEventReadModel
    {
        public Guid Id { get; set; }
        
        public string EventType { get; set; }
        
        public dynamic Event { get; set; }
        
        public DateTimeOffset TimeStamp { get; set; }
    }
}