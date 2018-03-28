namespace RideSharing.RideApi.Model
{
    public class Ride
    {
        public Ride()
        {
            Status = RideState.Requested;
        }
        
        public RideState Status { get; }
    }
}