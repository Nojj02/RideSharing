using System;

namespace RideSharing.RideApi.Model
{
    public class Ride : Entity
    {
        public Ride(Guid id, string pickupPoint, string destination)
            : base(id)
        {
            Status = RideState.Requested;
            PickupPoint = pickupPoint;
            Destination = destination;
        }
        
        public RideState Status { get; }

        public string PickupPoint { get; set; }

        public string Destination { get; set; }
    }
}