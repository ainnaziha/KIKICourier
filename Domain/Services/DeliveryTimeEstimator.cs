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

            var maxDistance = shipment.MaxDistance;
            var travelTime = maxDistance / availableVehicle.MaxSpeedKmPerHour;

            foreach (var package in shipment.Packages)
            {
                var packageTravelTime = package.DistanceKm / availableVehicle.MaxSpeedKmPerHour;
                deliveryTimes[package.Id] = availableVehicle.AvailableAtHours + packageTravelTime;
                remainingPackages.Remove(package);
            }

            availableVehicle.AvailableAtHours += (2 * travelTime);
        }

        return deliveryTimes;
    }
}
