using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Services;

namespace KIKICourier.Application;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryCostCalculator _costCalculator;
    private readonly IDeliveryTimeEstimator _timeEstimator;

    public DeliveryService(
        IDeliveryCostCalculator costCalculator,
        IDeliveryTimeEstimator timeEstimator)
    {
        _costCalculator = costCalculator ?? throw new ArgumentNullException(nameof(costCalculator));
        _timeEstimator = timeEstimator ?? throw new ArgumentNullException(nameof(timeEstimator));
    }

    public List<DeliveryCostResult> ProcessDeliveryEstimation(
        double baseDeliveryCost,
        List<Package> packages)
    {
        if (packages == null || packages.Count == 0)
            throw new ArgumentException("Packages list cannot be empty", nameof(packages));

        var results = new List<DeliveryCostResult>();

        foreach (var package in packages)
        {
            var result = _costCalculator.CalculateWithDiscount(baseDeliveryCost, package);
            results.Add(result);
        }

        return results;
    }

    public List<DeliveryCostResult> ProcessDeliveryWithTimeEstimation(
        double baseDeliveryCost,
        List<Package> packages,
        int numberOfVehicles,
        double maxSpeedKmPerHour,
        double maxCarriableWeightKg)
    {
        if (packages == null || packages.Count == 0)
            throw new ArgumentException("Packages list cannot be empty", nameof(packages));
        if (numberOfVehicles <= 0)
            throw new ArgumentException("Number of vehicles must be positive", nameof(numberOfVehicles));

        var results = ProcessDeliveryEstimation(baseDeliveryCost, packages);

        var vehicles = new List<Vehicle>();
        for (int i = 1; i <= numberOfVehicles; i++)
        {
            vehicles.Add(new Vehicle(i, maxSpeedKmPerHour, maxCarriableWeightKg));
        }

        var deliveryTimes = _timeEstimator.EstimateDeliveryTimes(
            packages,
            vehicles,
            maxCarriableWeightKg);

        foreach (var result in results)
        {
            if (deliveryTimes.TryGetValue(result.PackageId, out double deliveryTime))
            {
                result.EstimatedDeliveryTimeHours = deliveryTime;
            }
        }

        return results;
    }
}
