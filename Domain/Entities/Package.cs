namespace KIKICourier.Domain.Entities;

public class Package
{
    public string Id { get; }
    public double WeightKg { get; }
    public double DistanceKm { get; }
    public string? OfferCode { get; }

    public Package(string id, double weightKg, double distanceKm, string? offerCode = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Package ID cannot be empty", nameof(id));
        if (weightKg <= 0)
            throw new ArgumentException("Weight must be positive", nameof(weightKg));
        if (distanceKm <= 0)
            throw new ArgumentException("Distance must be positive", nameof(distanceKm));

        Id = id;
        WeightKg = weightKg;
        DistanceKm = distanceKm;
        OfferCode = string.IsNullOrWhiteSpace(offerCode) || offerCode.Equals("NA", StringComparison.OrdinalIgnoreCase)
            ? null
            : offerCode;
    }
}
