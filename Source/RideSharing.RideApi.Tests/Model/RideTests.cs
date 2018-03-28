using RideSharing.RideApi.Model;
using Xunit;

namespace RideSharing.RideApi.Tests.Model
{
    public class RideTests
    {
        [Fact]
        public void Test1()
        {
            var ride = new Ride();

            Assert.Equal(RideState.Requested, ride.Status);
        }
    }
}