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
        try
        {
            var (baseDeliveryCost, packages) = ReadInput();

            Console.WriteLine("\nDo you want to estimate delivery time? (y/n)");
            var estimateTime = Console.ReadLine()?.Trim().ToLower() == "y";

            List<DeliveryCostResult> results;

            if (estimateTime)
            {
                var (numberOfVehicles, maxSpeed, maxWeight) = ReadVehicleInput();
                results = _deliveryService.ProcessDeliveryWithTimeEstimation(
                    baseDeliveryCost,
                    packages,
                    numberOfVehicles,
                    maxSpeed,
                    maxWeight);
            }
            else
            {
                results = _deliveryService.ProcessDeliveryEstimation(baseDeliveryCost, packages);
            }

            DisplayResults(results, estimateTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static (double baseDeliveryCost, List<Package> packages) ReadInput()
    {
        var firstLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstLine))
            throw new InvalidOperationException("Input cannot be empty");

        var parts = firstLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            throw new InvalidOperationException("First line must contain: base_delivery_cost no_of_packages");

        double baseDeliveryCost = double.Parse(parts[0]);
        int numberOfPackages = int.Parse(parts[1]);

        var packages = new List<Package>();

        for (int i = 0; i < numberOfPackages; i++)
        {
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
