using KIKICourier.Domain.Entities;

namespace KIKICourier.Domain.Services;

public interface IDeliveryCostCalculator
{
    double CalculateBaseCost(double baseDeliveryCost, Package package);
    double CalculateDiscount(Package package, double deliveryCost);
    DeliveryCostResult CalculateWithDiscount(double baseDeliveryCost, Package package);
}
