﻿using System;

namespace RideSharing.RideApi.DataAccess
{
    public class StoredEvent
    { 
        public Guid Id { get; set; }
        
        public int Version { get; set; }
             
        public string EventType { get; set; }
             
        public object Event { get; set; }
             
        public DateTimeOffset TimeStamp { get; set; }
    }
}