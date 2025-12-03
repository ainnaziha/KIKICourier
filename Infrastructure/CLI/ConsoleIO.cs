using KIKICourier.Application;
using KIKICourier.Domain.Entities;

namespace KIKICourier.Infrastructure.CLI;

public class ConsoleIO
{
    private readonly IDeliveryService _deliveryService;

    public ConsoleIO(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService ?? throw new ArgumentNullException(nameof(deliveryService));
    }

    public void Run()
    {
        while (true)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== KIKI Courier At Your Service ^_^ ===");
                Console.WriteLine("1. Delivery Cost Estimation with Offers");
                Console.WriteLine("2. Delivery Time Estimation");
                Console.WriteLine("3. Exit");
                Console.Write("\nChoose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EstimateCost();
                        break;

                    case "2":
                        EstimateTime();
                        break;

                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void EstimateCost()
    {
        Console.Clear();
        Console.WriteLine("=== Estimate Delivery Cost ===\n");

        var (baseDeliveryCost, packages) = ReadInput();

        var results = _deliveryService.ProcessDeliveryEstimation(baseDeliveryCost, packages);

        Console.WriteLine("\n=== Results ===");
        DisplayResults(results, false);

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }

    private void EstimateTime()
    {
        Console.Clear();
        Console.WriteLine("=== Estimate Delivery Time ===\n");

        var (baseDeliveryCost, packages) = ReadInput();
        var (numberOfVehicles, maxSpeed, maxWeight) = ReadVehicleInput();

        var results = _deliveryService.ProcessDeliveryWithTimeEstimation(
            baseDeliveryCost,
            packages,
            numberOfVehicles,
            maxSpeed,
            maxWeight);

        Console.WriteLine("\n=== Results ===");
        DisplayResults(results, true);

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }

    private static (double baseDeliveryCost, List<Package> packages) ReadInput()
    {
        Console.Write("Enter base delivery cost and number of packages (e.g., 100 3): ");
        var firstLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstLine))
            throw new InvalidOperationException("Input cannot be empty");

        var parts = firstLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            throw new InvalidOperationException("First line must contain: base_delivery_cost no_of_packages");

        double baseDeliveryCost = double.Parse(parts[0]);
        int numberOfPackages = int.Parse(parts[1]);

        var packages = new List<Package>();

        Console.WriteLine("\nEnter package details (format: pkg_id weight distance offer_code):");
        for (int i = 0; i < numberOfPackages; i++)
        {
            Console.Write($"Package {i + 1}: ");
            var packageLine = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(packageLine))
                throw new InvalidOperationException($"Package line {i + 1} cannot be empty");

            var packageParts = packageLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (packageParts.Length != 4)
                throw new InvalidOperationException($"Package line must contain: pkg_id weight distance offer_code");

            string packageId = packageParts[0];
            double weight = double.Parse(packageParts[1]);
            double distance = double.Parse(packageParts[2]);
            string? offerCode = packageParts[3];

            packages.Add(new Package(packageId, weight, distance, offerCode));
        }

        return (baseDeliveryCost, packages);
    }

    private static (int numberOfVehicles, double maxSpeed, double maxWeight) ReadVehicleInput()
    {
        Console.Write("\nEnter vehicle details (format: no_of_vehicles max_speed max_weight): ");
        var vehicleLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(vehicleLine))
            throw new InvalidOperationException("Vehicle line cannot be empty");

        var parts = vehicleLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
            throw new InvalidOperationException("Vehicle line must contain: no_of_vehicles max_speed max_carriable_weight");

        int numberOfVehicles = int.Parse(parts[0]);
        double maxSpeed = double.Parse(parts[1]);
        double maxWeight = double.Parse(parts[2]);

        return (numberOfVehicles, maxSpeed, maxWeight);
    }

    private static void DisplayResults(List<DeliveryCostResult> results, bool includeTime)
    {
        foreach (var result in results)
        {
            if (includeTime && result.EstimatedDeliveryTimeHours.HasValue)
            {
                Console.WriteLine($"{result.PackageId} {result.Discount} {result.TotalCost} {result.EstimatedDeliveryTimeHours.Value:F2}");
            }
            else
            {
                Console.WriteLine($"{result.PackageId} {result.Discount} {result.TotalCost}");
            }
        }
    }
}
