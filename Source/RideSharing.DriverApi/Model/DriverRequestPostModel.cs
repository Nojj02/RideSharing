using System;

namespace RideSharing.DriverApi.Model
{
    public class DriverRequestPostModel
    {
        public Guid RideId { get; set; }

        public string PickupPoint { get; set; }
    }
}