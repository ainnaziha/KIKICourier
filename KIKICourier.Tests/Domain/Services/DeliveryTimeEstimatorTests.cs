using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Services;

namespace KIKICourier.Tests.Domain.Services;

public class DeliveryTimeEstimatorTests
{
    private readonly IShipmentOptimizer _optimizer;
    private readonly IDeliveryTimeEstimator _estimator;

    public DeliveryTimeEstimatorTests()
    {
        _optimizer = new ShipmentOptimizer();
        _estimator = new DeliveryTimeEstimator(_optimizer);
    }

    [Fact]
    public void EstimateDeliveryTimes_WithEmptyPackages_ReturnsEmptyDictionary()
    {
        var packages = new List<Package>();
        var vehicles = new List<Vehicle> { new Vehicle(1, 70, 200) };

        var times = _estimator.EstimateDeliveryTimes(packages, vehicles, 200);

        Assert.Empty(times);
    }

    [Fact]
    public void EstimateDeliveryTimes_WithNoVehicles_ThrowsArgumentException()
    {
        var packages = new List<Package> { new Package("PKG1", 50, 30, null) };
        var vehicles = new List<Vehicle>();

        Assert.Throws<ArgumentException>(() =>
            _estimator.EstimateDeliveryTimes(packages, vehicles, 200));
    }

    [Fact]
    public void EstimateDeliveryTimes_WithSinglePackage_ReturnsCorrectTime()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 70, null)
        };
        var vehicles = new List<Vehicle> { new Vehicle(1, 70, 200) };

        var times = _estimator.EstimateDeliveryTimes(packages, vehicles, 200);

        Assert.Single(times);
        Assert.Equal(1.0, times["PKG1"], 2); // 70km / 70km/h = 1.0 hour
    }

    [Fact]
    public void EstimateDeliveryTimes_UsesAvailableVehicleFirst()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null),
            new Package("PKG2", 75, 125, null)
        };
        var vehicles = new List<Vehicle>
        {
            new Vehicle(1, 70, 200),
            new Vehicle(2, 70, 200)
        };

        var times = _estimator.EstimateDeliveryTimes(packages, vehicles, 200);

        Assert.True(times["PKG1"] < 1);
        Assert.True(times["PKG2"] < 2);
    }

    [Fact]
    public void EstimateDeliveryTimes_TestCase2_ReturnsExpectedTimes()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null),
            new Package("PKG2", 75, 125, null),
            new Package("PKG3", 175, 100, null),
            new Package("PKG4", 110, 60, null),
            new Package("PKG5", 155, 95, null)
        };
        var vehicles = new List<Vehicle>
        {
            new Vehicle(1, 70, 200),
            new Vehicle(2, 70, 200)
        };

        var times = _estimator.EstimateDeliveryTimes(packages, vehicles, 200);

        Assert.Equal(5, times.Count);
        Assert.InRange(times["PKG4"], 0.84, 0.86);
        Assert.InRange(times["PKG2"], 1.77, 1.79);
        Assert.InRange(times["PKG3"], 1.41, 1.43);
        Assert.InRange(times["PKG1"], 3.97, 3.99);
        Assert.InRange(times["PKG5"], 4.18, 4.22);
    }

    [Fact]
    public void EstimateDeliveryTimes_VehicleReturnTime_BasedOnTruncatedDeliveryTime()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 110, 60, null),
            new Package("PKG2", 110, 70, null)
        };
        var vehicles = new List<Vehicle>
        {
            new Vehicle(1, 70, 200)
        };

        var times = _estimator.EstimateDeliveryTimes(packages, vehicles, 200);

        // Both packages fit in one shipment (110 + 110 = 220 > 200), so they go separately
        // PKG1: 60/70 = 0.857... truncated to 0.85
        Assert.InRange(times["PKG1"], 0.84, 0.86);

        // Vehicle returns at 2 * 0.85 = 1.70
        // PKG2: 1.70 + 70/70 = 1.70 + 1.0 = 2.70
        Assert.InRange(times["PKG2"], 2.69, 2.71);
    }

    [Fact]
    public void EstimateDeliveryTimes_AllPackagesGetDeliveryTime()
    {
        var packages = new List<Package>
        {
            new Package("PKG1", 50, 30, null),
            new Package("PKG2", 75, 125, null),
            new Package("PKG3", 175, 100, null)
        };
        var vehicles = new List<Vehicle>
        {
            new Vehicle(1, 70, 200)
        };

        var times = _estimator.EstimateDeliveryTimes(packages, vehicles, 200);

        Assert.Equal(3, times.Count);
        Assert.Contains("PKG1", times.Keys);
        Assert.Contains("PKG2", times.Keys);
        Assert.Contains("PKG3", times.Keys);
    }
}
