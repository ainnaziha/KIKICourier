using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Repositories;

namespace KIKICourier.Domain.Services;

public class DeliveryCostCalculator : IDeliveryCostCalculator
{
    private const double WeightMultiplier = 10;
    private const double DistanceMultiplier = 5;

    private readonly IOfferRepository _offerRepository;

    public DeliveryCostCalculator(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
    }

    public double CalculateBaseCost(double baseDeliveryCost, Package package)
    {
        if (baseDeliveryCost < 0)
            throw new ArgumentException("Base delivery cost cannot be negative", nameof(baseDeliveryCost));
        if (package == null)
            throw new ArgumentNullException(nameof(package));

        return baseDeliveryCost +
               (package.WeightKg * WeightMultiplier) +
               (package.DistanceKm * DistanceMultiplier);
    }

    public double CalculateDiscount(Package package, double deliveryCost)
    {
        if (package == null)
            throw new ArgumentNullException(nameof(package));
        if (string.IsNullOrWhiteSpace(package.OfferCode))
            return 0;

        var offer = _offerRepository.GetByCode(package.OfferCode);
        if (offer == null || !offer.IsApplicable(package))
            return 0;

        return deliveryCost * (offer.DiscountPercentage / 100);
    }

    public DeliveryCostResult CalculateWithDiscount(double baseDeliveryCost, Package package)
    {
        if (package == null)
            throw new ArgumentNullException(nameof(package));

        double deliveryCost = CalculateBaseCost(baseDeliveryCost, package);
        double discount = CalculateDiscount(package, deliveryCost);
        double totalCost = deliveryCost - discount;

        return new DeliveryCostResult(package.Id, discount, totalCost);
    }
}
