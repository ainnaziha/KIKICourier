using KIKICourier.Domain.Entities;

namespace KIKICourier.Application;

public interface IDeliveryService
{
    List<DeliveryCostResult> ProcessDeliveryEstimation(
        double baseDeliveryCost,
        List<Package> packages);

    List<DeliveryCostResult> ProcessDeliveryWithTimeEstimation(
        double baseDeliveryCost,
        List<Package> packages,
        int numberOfVehicles,
        double maxSpeedKmPerHour,
        double maxCarriableWeightKg);
}
