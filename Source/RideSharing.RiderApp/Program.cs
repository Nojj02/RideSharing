using System;
using System.Globalization;
using System.Net.Cache;
using System.Threading.Tasks;

namespace RideSharing.RiderApp
{
    class Program
    {
        static void Main(string[] args)
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
                        RequestRide();
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

        private static void RequestRide()
        {
            Console.WriteLine("Request ride.");
            
        }
    }
}
