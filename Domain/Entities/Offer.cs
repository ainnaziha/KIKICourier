namespace KIKICourier.Domain.Entities;

public class Offer
{
    public string Code { get; }
    public double DiscountPercentage { get; }
    public double MinDistanceKm { get; }
    public double MaxDistanceKm { get; }
    public double MinWeightKg { get; }
    public double MaxWeightKg { get; }

    public Offer(
        string code,
        double discountPercentage,
        double minDistanceKm,
        double maxDistanceKm,
        double minWeightKg,
        double maxWeightKg)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Offer code cannot be empty", nameof(code));
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));
        if (minDistanceKm < 0)
            throw new ArgumentException("Min distance cannot be negative", nameof(minDistanceKm));
        if (maxDistanceKm < minDistanceKm)
            throw new ArgumentException("Max distance must be >= min distance", nameof(maxDistanceKm));
        if (minWeightKg < 0)
            throw new ArgumentException("Min weight cannot be negative", nameof(minWeightKg));
        if (maxWeightKg < minWeightKg)
            throw new ArgumentException("Max weight must be >= min weight", nameof(maxWeightKg));

        Code = code;
        DiscountPercentage = discountPercentage;
        MinDistanceKm = minDistanceKm;
        MaxDistanceKm = maxDistanceKm;
        MinWeightKg = minWeightKg;
        MaxWeightKg = maxWeightKg;
    }

    public bool IsApplicable(Package package)
    {
        return package.DistanceKm >= MinDistanceKm &&
               package.DistanceKm < MaxDistanceKm &&
               package.WeightKg >= MinWeightKg &&
               package.WeightKg <= MaxWeightKg;
    }
}
