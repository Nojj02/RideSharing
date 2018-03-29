using System;

namespace RideSharing.DriverApi.Model
{
    public class DriverRequest
    {
        public DriverRequest(Guid rideId, string pickupPoint)
        {
            RideId = rideId;
            Id = Guid.NewGuid();
            PickupPoint = pickupPoint;
            Status = DriverRequestStatus.Pending;
        }

        public Guid RideId { get; }

        public Guid Id { get; }
        
        public string PickupPoint { get; }
        
        public DriverRequestStatus Status { get; }
    }
}