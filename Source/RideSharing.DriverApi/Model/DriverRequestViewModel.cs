using System;
using System.Data.Common;

namespace RideSharing.DriverApi.Model
{
    public class DriverRequestViewModel
    {
        public DriverRequestViewModel(DriverRequest request)
        {
            Id = request.Id;
            RideId = request.RideId;
            PickupPoint = request.PickupPoint;
            Status = request.Status;
        }

        public Guid Id { get; }
        
        public Guid RideId { get; }
        
        public string PickupPoint { get; }
        
        public DriverRequestStatus Status { get; }
    }
}