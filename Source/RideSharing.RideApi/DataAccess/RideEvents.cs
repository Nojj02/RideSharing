using System;
using System.Collections.Generic;
using RideSharing.RideApi.Model;

namespace RideSharing.RideApi.DataAccess
{
    public static class RideEvents
    {
        public static readonly IReadOnlyDictionary<Type, string> EventTypeLookup =
            new Dictionary<Type, string>
            {
                { typeof(RideRequestedEvent), "RideSharing.RideApi.Model.RideRequestedEvent" },
                { typeof(RideAcceptedEvent), "RideSharing.RideApi.Model.RideAcceptedEvent" }
            };
    }
}