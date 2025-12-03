using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Services;

namespace KIKICourier.Tests.Domain.Services;

public class ShipmentOptimizerTests
{
    private readonly IShipmentOptimizer _optimizer;

    public ShipmentOptimizerTests()
    {
        _optimizer = new ShipmentOptimizer();
    }

    [Fact]
    public void OptimizeShipments_WithEmptyPackages_ReturnsEmptyList()
    {
        var packages = new List<Package>();

        var shipments = _optimizer.OptimizeShipments(packages, 200);

        Assert.Empty(shipments);
    }

    [Fact]
    public void OptimizeShipments_WithSinglePackage_ReturnsSingleShipment()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null)
        };

        var shipments = _optimizer.OptimizeShipments(packages, 200);

        Assert.Single(shipments);
        Assert.Single(shipments[0].Packages);
        Assert.Equal("PKG1", shipments[0].Packages[0].Id);
    }

    [Fact]
    public void OptimizeShipments_PrioritizesMaxPackages()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null),
            new Package("PKG2", 75, 125, null),
            new Package("PKG3", 175, 100, null)
        };

        var shipments = _optimizer.OptimizeShipments(packages, 200);

        var firstShipment = shipments[0];
        Assert.Equal(2, firstShipment.Packages.Count);
    }

    [Fact]
    public void OptimizeShipments_WhenEqualPackages_PrioritizesHeavier()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null),
            new Package("PKG2", 75, 125, null),
            new Package("PKG3", 110, 60, null)
        };

        var shipments = _optimizer.OptimizeShipments(packages, 200);

        var firstShipment = shipments[0];
        Assert.Equal(2, firstShipment.Packages.Count);
        Assert.Equal(185, firstShipment.TotalWeight); // PKG2 (75) + PKG3 (110)
    }

    [Fact]
    public void OptimizeShipments_WhenEqualPackagesAndWeight_PrioritizesShorterDistance()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 100, 100, null),
            new Package("PKG2", 100, 50, null)
        };

        var shipments = _optimizer.OptimizeShipments(packages, 150);

        var firstShipment = shipments[0];
        Assert.Single(firstShipment.Packages);
        Assert.Equal("PKG2", firstShipment.Packages[0].Id); // Shorter distance
    }

    [Fact]
    public void OptimizeShipments_RespectsWeightConstraint()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 150, 30, null),
            new Package("PKG2", 100, 50, null)
        };

        var shipments = _optimizer.OptimizeShipments(packages, 200);

        foreach (var shipment in shipments)
        {
            Assert.True(shipment.TotalWeight <= 200);
        }
    }

    [Fact]
    public void OptimizeShipments_HandlesAllPackages()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null),
            new Package("PKG2", 75, 125, null),
            new Package("PKG3", 175, 100, null),
            new Package("PKG4", 110, 60, null),
            new Package("PKG5", 155, 95, null)
        };

        var shipments = _optimizer.OptimizeShipments(packages, 200);

        var allPackageIds = shipments.SelectMany(s => s.Packages).Select(p => p.Id).ToList();
        Assert.Equal(5, allPackageIds.Count);
        Assert.Contains("PKG1", allPackageIds);
        Assert.Contains("PKG2", allPackageIds);
        Assert.Contains("PKG3", allPackageIds);
        Assert.Contains("PKG4", allPackageIds);
        Assert.Contains("PKG5", allPackageIds);
    }
}
