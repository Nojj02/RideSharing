using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RideSharing.RideMatcher
{
    class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        private static readonly List<Guid> ProcessedRides = new List<Guid>();

        static async Task Main(string[] args)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Going to match");

            while (true)
            {
                //await ListenToRideRequest();
                await Task.Delay(1000);
            }
        }

//        private static async Task ListenToRideRequest()
//        {
//            var response = await Client.GetStringAsync("http://localhost:5000/events/rideevent");
//            var events = JsonConvert.DeserializeObject<IEnumerable<StoredEventReadModel>>(response);
//
//            foreach (var anEvent in events)
//            {
//                if (!ProcessedRides.Contains(anEvent.Id) &&
//                    anEvent.EventType == "RideSharing.RideApi.Model.RideRequestedEvent")
//                {
//                    Console.WriteLine($"Event found {anEvent.EventType}");
//                    
//                    var rideRequestedEvent = JsonConvert.DeserializeObject<RideRequestedEvent>(JsonConvert.SerializeObject(anEvent.Event));
//                    
//                    // do the magic
//                    var driverRequest =
//                        new
//                        {
//                            RideId = rideRequestedEvent.Id,
//                            PickupPoint = rideRequestedEvent.PickupPoint
//                        };
//
//                    ProcessedRides.Add(rideRequestedEvent.Id);
//                    
//                    var driverRequestResponse = 
//                        await Client.PostAsync("http://localhost:5001/api/driverrequest", 
//                            new StringContent(JsonConvert.SerializeObject(driverRequest), Encoding.UTF8, "application/json"));
//                    
//                    Console.WriteLine(driverRequestResponse.StatusCode);
//                }
//            }
//        }
    }

    public class RideRequestedEvent : RideEvent
    {
        public RideRequestedEvent(
            string pickupPoint,
            string destination)
        {
            PickupPoint = pickupPoint;
            Destination = destination;
        }

        public string PickupPoint { get; }

        public string Destination { get; }
    }

    public class RideAcceptedEvent : RideEvent
    {
    }

    public class RideEvent
    {
        public Guid Id { get; set; }

        public int Version { get; set; }
    }
}