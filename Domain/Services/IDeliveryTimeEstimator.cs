using KIKICourier.Domain.Entities;

namespace KIKICourier.Domain.Services;

public interface IDeliveryTimeEstimator
{
    Dictionary<string, double> EstimateDeliveryTimes(
        List<Package> packages,
        List<Vehicle> vehicles,
        double maxCarriableWeight);
}
