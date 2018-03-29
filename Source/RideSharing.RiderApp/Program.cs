using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RideSharing.RiderApp
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to this Ride Sharing app!");

            var shouldQuit = false;

            while (!shouldQuit)
            {
                ShowMenu();
                var selectedOption = Console.ReadLine();

                switch (selectedOption)
                {
                    case "1":
                        await RequestRide();
                        break;
                    case "q":
                        shouldQuit = true;
                        break;
                    default:
                        Console.WriteLine("Unknown option selected. Try again");
                        break;
                }
            }
            
            Console.WriteLine("Exiting app..");
        }

        private static void ShowMenu()
        {
            Console.WriteLine("MENU");
            Console.WriteLine("================");
            Console.WriteLine("[1] Request Ride");
            Console.WriteLine("[q] Quit");
            Console.WriteLine("SELECT AN OPTION");
        }

        private static async Task RequestRide()
        {
            Console.WriteLine("Request ride.");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Enter Pick-up Point:");
            var pickupPoint = Console.ReadLine();
            
            Console.WriteLine("Enter Destination:");
            var destination = Console.ReadLine();
            
            var ride = new { PickupPoint = pickupPoint, Destination = destination };
            var response = await client.PostAsync("http://localhost:5000/api/values", new StringContent(JsonConvert.SerializeObject(ride), Encoding.UTF8, "application/json"));
            
            Console.WriteLine(response.RequestMessage);
        }
    }
}
