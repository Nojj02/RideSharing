using System;

namespace RideSharing.RideApi.Model
{
    public class Ride : Entity
    {
        public Ride(Guid id, string pickupPoint, string destination)
            : base(id)
        {
            State = RideState.Requested;
            PickupPoint = pickupPoint;
            Destination = destination;
        }
        
        public RideState State { get; private set; }

        public string PickupPoint { get; }

        public string Destination { get; }

        public void Accept()
        {
            State = RideState.Accepted;
        }
    }
}