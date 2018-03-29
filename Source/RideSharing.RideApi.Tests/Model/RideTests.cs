using System;
using RideSharing.RideApi.Model;
using Xunit;

namespace RideSharing.RideApi.Tests.Model
{
    public class RideTests
    {
        [Fact]
        public void New_Ride()
        {
            var ride = new Ride(Guid.NewGuid(), "123 subdivision", "SM Mega Mall");

            Assert.Equal(RideState.Requested, ride.State);
            Assert.Equal("123 subdivision", ride.PickupPoint);
            Assert.Equal("SM Mega Mall", ride.Destination);
        }

        [Fact]
        public void Accept_Ride()
        {
            var ride = new Ride(Guid.NewGuid(), "123 subdivision", "SM Mega Mall");
            ride.Accept();
            
            Assert.Equal(RideState.Accepted, ride.State);
        }
    }
}