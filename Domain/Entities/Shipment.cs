namespace KIKICourier.Domain.Entities;

public class Shipment
{
    public List<Package> Packages { get; }
    public double TotalWeight => Packages.Sum(p => p.WeightKg);
    public double MaxDistance => Packages.Max(p => p.DistanceKm);

    public Shipment()
    {
        Packages = new List<Package>();
    }

    public Shipment(IEnumerable<Package> packages)
    {
        Packages = new List<Package>(packages);
    }

    public bool CanAdd(Package package, double maxCarriableWeight)
    {
        return TotalWeight + package.WeightKg <= maxCarriableWeight;
    }

    public void Add(Package package)
    {
        Packages.Add(package);
    }
}
