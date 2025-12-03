using KIKICourier.Application;
using KIKICourier.Domain.Entities;

namespace KIKICourier.Infrastructure.CLI
{
    public class ConsoleIO
    {
        private readonly IDeliveryService _deliveryService;

        public ConsoleIO(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== KIKI Courier At Your Service ^_^ ===");
                Console.WriteLine("1. Calculate Delivery Cost");
                Console.WriteLine("2. Exit");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CalculateDeliveryCost();
                        break;

                    case "2":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CalculateDeliveryCost()
        {
            Console.Clear();
            Console.WriteLine("=== Delivery Cost Estimation with Offers ===");

            double distance = ReadDouble("Enter distance (km): ");
            double weight = ReadDouble("Enter weight (kg): ");

            bool applyOffer = AskYesNo("Apply offer? (y/n): ");

            double cost = _deliveryService.ComputeDeliveryCost(distance, weight, applyOffer);

            Console.WriteLine($"\nTotal Delivery Cost: RM {cost:F2}");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
        private bool AskYesNo(string label)
        {
            while (true)
            {
                Console.Write(label);
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y") return true;
                if (input == "n") return false;

                Console.WriteLine("Please enter y or n.");
            }
        }

        private double ReadDouble(string label)
        {
            while (true)
            {
                Console.Write(label);
                string? input = Console.ReadLine();

                if (double.TryParse(input, out double value) && value > 0)
                {
                    return value;
                }

                Console.WriteLine("Invalid input, please enter a valid positive number.");
            }
        }
    }
}
