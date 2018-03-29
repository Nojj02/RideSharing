using System;
using RideSharing.DriverApi.Model;
using Xunit;

namespace RideSharing.DriverApi.Tests
{
    public class DriverRequestTests
    {
        [Fact]
        public void Creation()
        {
            //setup
            var driverRequest = new DriverRequest(
                rideId: new Guid("12345678-1234-1234-1234-123456789012"), 
                pickupPoint: "megamall"
            );
            
            //verify
            Assert.Equal(driverRequest.RideId, new Guid("12345678-1234-1234-1234-123456789012"));
            Assert.Equal(driverRequest.PickupPoint, "megamall");
            Assert.Equal(driverRequest.Status, DriverRequestStatus.Pending);   
        }
    }
}