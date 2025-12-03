using KIKICourier.Domain.Entities;

namespace KIKICourier.Domain.Services;

public class ShipmentOptimizer : IShipmentOptimizer
{
    public List<Shipment> OptimizeShipments(List<Package> packages, double maxCarriableWeight)
    {
        if (packages == null || packages.Count == 0)
            return new List<Shipment>();

        var shipments = new List<Shipment>();
        var remainingPackages = new List<Package>(packages);

        while (remainingPackages.Count > 0)
        {
            var bestShipment = FindBestShipment(remainingPackages, maxCarriableWeight);
            if (bestShipment.Packages.Count == 0)
                break;

            shipments.Add(bestShipment);

            foreach (var package in bestShipment.Packages)
            {
                remainingPackages.Remove(package);
            }
        }

        return shipments;
    }

    private static Shipment FindBestShipment(List<Package> availablePackages, double maxCarriableWeight)
    {
        var allCombinations = GenerateCombinations(availablePackages, maxCarriableWeight);

        if (allCombinations.Count == 0)
            return new Shipment();

        return allCombinations
            .OrderByDescending(s => s.Packages.Count)
            .ThenByDescending(s => s.TotalWeight)
            .ThenBy(s => s.MaxDistance)
            .First();
    }

    private static List<Shipment> GenerateCombinations(List<Package> packages, double maxWeight)
    {
        var validShipments = new List<Shipment>();
        int n = packages.Count;
        int totalCombinations = 1 << n;

        for (int i = 1; i < totalCombinations; i++)
        {
            var shipment = new Shipment();

            for (int j = 0; j < n; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    if (shipment.CanAdd(packages[j], maxWeight))
                    {
                        shipment.Add(packages[j]);
                    }
                    else
                    {
                        shipment = null;
                        break;
                    }
                }
            }

            if (shipment != null && shipment.Packages.Count > 0)
            {
                validShipments.Add(shipment);
            }
        }

        return validShipments;
    }
}
