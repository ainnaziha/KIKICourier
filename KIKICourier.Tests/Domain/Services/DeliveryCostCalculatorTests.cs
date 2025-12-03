using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Repositories;
using KIKICourier.Domain.Services;
using KIKICourier.Infrastructure.Repositories;

namespace KIKICourier.Tests.Domain.Services;

public class DeliveryCostCalculatorTests
{
    private readonly IOfferRepository _offerRepository;
    private readonly IDeliveryCostCalculator _calculator;

    public DeliveryCostCalculatorTests()
    {
        _offerRepository = new InMemoryOfferRepository();
        _calculator = new DeliveryCostCalculator(_offerRepository);
    }

    [Fact]
    public void CalculateBaseCost_WithValidInputs_ReturnsCorrectCost()
    {
        var package = new Package("PKG1", 5, 5, null);

        var cost = _calculator.CalculateBaseCost(100, package);

        Assert.Equal(175, cost); // 100 + (5 * 10) + (5 * 5)
    }

    [Fact]
    public void CalculateWithDiscount_WhenOfferNotApplicable_ReturnsZeroDiscount()
    {
        var package = new Package("PKG1", 5, 5, "OFR001");

        var result = _calculator.CalculateWithDiscount(100, package);

        Assert.Equal("PKG1", result.PackageId);
        Assert.Equal(0, result.Discount);
        Assert.Equal(175, result.TotalCost);
    }

    [Fact]
    public void CalculateWithDiscount_WhenOfferApplicable_ReturnsCorrectDiscount()
    {
        var package = new Package("PKG3", 10, 100, "OFR003");

        var result = _calculator.CalculateWithDiscount(100, package);

        Assert.Equal("PKG3", result.PackageId);
        Assert.Equal(35, result.Discount); // 700 * 0.05
        Assert.Equal(665, result.TotalCost); // 700 - 35
    }

    [Fact]
    public void CalculateWithDiscount_WhenNoOfferCode_ReturnsZeroDiscount()
    {
        var package = new Package("PKG1", 15, 5, null);

        var result = _calculator.CalculateWithDiscount(100, package);

        Assert.Equal(0, result.Discount);
        Assert.Equal(275, result.TotalCost);
    }

    [Fact]
    public void CalculateWithDiscount_WhenInvalidOfferCode_ReturnsZeroDiscount()
    {
        var package = new Package("PKG1", 50, 30, "INVALID");

        var result = _calculator.CalculateWithDiscount(100, package);

        Assert.Equal(0, result.Discount);
        Assert.Equal(750, result.TotalCost);
    }

    [Theory]
    [InlineData("PKG1", 5, 5, "OFR001", 100, 0, 175)]
    [InlineData("PKG2", 15, 5, "OFR002", 100, 0, 275)]
    [InlineData("PKG3", 10, 100, "OFR003", 100, 35, 665)]
    public void CalculateWithDiscount_TestCase1Scenarios_ReturnsExpectedResults(
        string id, double weight, double distance, string offerCode,
        double baseCost, double expectedDiscount, double expectedTotal)
    {
        var package = new Package(id, weight, distance, offerCode);

        var result = _calculator.CalculateWithDiscount(baseCost, package);

        Assert.Equal(expectedDiscount, result.Discount);
        Assert.Equal(expectedTotal, result.TotalCost);
    }

    [Theory]
    [InlineData("PKG1", 50, 30, "OFR001", 100, 0, 750)]
    [InlineData("PKG4", 110, 60, "OFR002", 100, 105, 1395)]
    public void CalculateWithDiscount_TestCase2Scenarios_ReturnsExpectedResults(
        string id, double weight, double distance, string offerCode,
        double baseCost, double expectedDiscount, double expectedTotal)
    {
        var package = new Package(id, weight, distance, offerCode);

        var result = _calculator.CalculateWithDiscount(baseCost, package);

        Assert.Equal(expectedDiscount, result.Discount, 2);
        Assert.Equal(expectedTotal, result.TotalCost, 2);
    }
}
