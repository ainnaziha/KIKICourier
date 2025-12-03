using KIKICourier.Domain.Entities;

namespace KIKICourier.Domain.Services;

public interface IShipmentOptimizer
{
    List<Shipment> OptimizeShipments(List<Package> packages, double maxCarriableWeight);
}
