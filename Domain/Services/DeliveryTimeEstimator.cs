using KIKICourier.Domain.Entities;

namespace KIKICourier.Domain.Services;

public class DeliveryTimeEstimator : IDeliveryTimeEstimator
{
    private readonly IShipmentOptimizer _shipmentOptimizer;

    public DeliveryTimeEstimator(IShipmentOptimizer shipmentOptimizer)
    {
        _shipmentOptimizer = shipmentOptimizer ?? throw new ArgumentNullException(nameof(shipmentOptimizer));
    }

    public Dictionary<string, double> EstimateDeliveryTimes(
        List<Package> packages,
        List<Vehicle> vehicles,
        double maxCarriableWeight)
    {
        if (packages == null || packages.Count == 0)
            return new Dictionary<string, double>();
        if (vehicles == null || vehicles.Count == 0)
            throw new ArgumentException("At least one vehicle is required", nameof(vehicles));

        var deliveryTimes = new Dictionary<string, double>();
        var remainingPackages = new List<Package>(packages);

        foreach (var vehicle in vehicles)
        {
            vehicle.AvailableAtHours = 0;
        }

        while (remainingPackages.Count > 0)
        {
            var availableVehicle = vehicles.OrderBy(v => v.AvailableAtHours).First();

            var shipments = _shipmentOptimizer.OptimizeShipments(remainingPackages, maxCarriableWeight);

            if (shipments.Count == 0)
                break;

            var shipment = shipments[0];

            if (shipment.Packages.Count == 0)
                break;

            // Console.WriteLine($"[DEBUG] Vehicle {availableVehicle.Id} (avail: {availableVehicle.AvailableAtHours:F3}) taking: {string.Join(", ", shipment.Packages.Select(p => p.Id))}");

            foreach (var package in shipment.Packages)
            {
                var packageTravelTime = package.DistanceKm / availableVehicle.MaxSpeedKmPerHour;
                deliveryTimes[package.Id] = availableVehicle.AvailableAtHours + packageTravelTime;
                // Console.WriteLine($"[DEBUG]   {package.Id}: {availableVehicle.AvailableAtHours:F3} + {packageTravelTime:F3} = {deliveryTimes[package.Id]:F3}");
                remainingPackages.Remove(package);
            }

            var maxDistance = shipment.MaxDistance;
            var roundTripTime = (2 * maxDistance) / availableVehicle.MaxSpeedKmPerHour;
            availableVehicle.AvailableAtHours += roundTripTime;
            // Console.WriteLine($"[DEBUG]   Vehicle {availableVehicle.Id} returns at: {availableVehicle.AvailableAtHours:F3}");
        }

        return deliveryTimes;
    }
}
