using KIKICourier.Application;
using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Repositories;
using KIKICourier.Domain.Services;
using KIKICourier.Infrastructure.Repositories;

namespace KIKICourier.Tests.Application;

public class DeliveryServiceTests
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryServiceTests()
    {
        IOfferRepository offerRepository = new InMemoryOfferRepository();
        IDeliveryCostCalculator costCalculator = new DeliveryCostCalculator(offerRepository);
        IShipmentOptimizer shipmentOptimizer = new ShipmentOptimizer();
        IDeliveryTimeEstimator timeEstimator = new DeliveryTimeEstimator(shipmentOptimizer);

        _deliveryService = new DeliveryService(costCalculator, timeEstimator);
    }

    [Fact]
    public void ProcessDeliveryEstimation_WithValidInput_ReturnsResults()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 5, 5, "OFR001"),
            new Package("PKG2", 15, 5, "OFR002"),
            new Package("PKG3", 10, 100, "OFR003")
        };

        var results = _deliveryService.ProcessDeliveryEstimation(100, packages);

        Assert.Equal(3, results.Count);
        Assert.All(results, r => Assert.Null(r.EstimatedDeliveryTimeHours));
    }

    [Fact]
    public void ProcessDeliveryEstimation_TestCase1_ReturnsExpectedResults()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 5, 5, "OFR001"),
            new Package("PKG2", 15, 5, "OFR002"),
            new Package("PKG3", 10, 100, "OFR003")
        };

        var results = _deliveryService.ProcessDeliveryEstimation(100, packages);

        var pkg1 = results.First(r => r.PackageId == "PKG1");
        Assert.Equal(0, pkg1.Discount);
        Assert.Equal(175, pkg1.TotalCost);

        var pkg2 = results.First(r => r.PackageId == "PKG2");
        Assert.Equal(0, pkg2.Discount);
        Assert.Equal(275, pkg2.TotalCost);

        var pkg3 = results.First(r => r.PackageId == "PKG3");
        Assert.Equal(35, pkg3.Discount);
        Assert.Equal(665, pkg3.TotalCost);
    }

    [Fact]
    public void ProcessDeliveryWithTimeEstimation_WithValidInput_ReturnsResultsWithTimes()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, "OFR001")
        };

        var results = _deliveryService.ProcessDeliveryWithTimeEstimation(100, packages, 1, 70, 200);

        Assert.Single(results);
        Assert.NotNull(results[0].EstimatedDeliveryTimeHours);
    }

    [Fact]
    public void ProcessDeliveryWithTimeEstimation_TestCase2_ReturnsExpectedResults()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, "OFR001"),
            new Package("PKG2", 75, 125, "OFR0008"),
            new Package("PKG3", 175, 100, "OFR003"),
            new Package("PKG4", 110, 60, "OFR002"),
            new Package("PKG5", 155, 95, "NA")
        };

        var results = _deliveryService.ProcessDeliveryWithTimeEstimation(100, packages, 2, 70, 200);

        Assert.Equal(5, results.Count);

        var pkg1 = results.First(r => r.PackageId == "PKG1");
        Assert.Equal(0, pkg1.Discount);
        Assert.Equal(750, pkg1.TotalCost);
        Assert.NotNull(pkg1.EstimatedDeliveryTimeHours);
        Assert.InRange(pkg1.EstimatedDeliveryTimeHours.Value, 3.97, 4.01);

        var pkg4 = results.First(r => r.PackageId == "PKG4");
        Assert.Equal(105, pkg4.Discount, 2);
        Assert.Equal(1395, pkg4.TotalCost, 2);
        Assert.InRange(pkg4.EstimatedDeliveryTimeHours.Value, 0.84, 0.86);
    }

    [Fact]
    public void ProcessDeliveryEstimation_WithEmptyPackages_ThrowsArgumentException()
    {
        var packages = new List<Package>();

        Assert.Throws<ArgumentException>(() =>
            _deliveryService.ProcessDeliveryEstimation(100, packages));
    }

    [Fact]
    public void ProcessDeliveryWithTimeEstimation_WithZeroVehicles_ThrowsArgumentException()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null)
        };

        Assert.Throws<ArgumentException>(() =>
            _deliveryService.ProcessDeliveryWithTimeEstimation(100, packages, 0, 70, 200));
    }
}
