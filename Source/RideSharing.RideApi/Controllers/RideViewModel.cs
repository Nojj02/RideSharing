using System;
using RideSharing.RideApi.Model;

namespace RideSharing.RideApi.Controllers
{
    public class RideViewModel 
    {
        public RideViewModel(Guid id, string pickupPoint, string destination, RideState state)
        {
            Id = id;
            PickupPoint = pickupPoint;
            Destination = destination;
            State = state;
        }
        
        public Guid Id { get; }

        public RideState State { get; }
        
        public string PickupPoint { get; }

        public string Destination { get; }
    }
}