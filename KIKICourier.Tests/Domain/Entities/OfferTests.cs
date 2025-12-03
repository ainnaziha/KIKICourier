using KIKICourier.Domain.Entities;

namespace KIKICourier.Tests.Domain.Entities;

public class OfferTests
{
    [Fact]
    public void IsApplicable_WhenPackageMeetsAllCriteria_ReturnsTrue()
    {
        var offer = new Offer("OFR003", 5, 50, 250, 10, 150);
        var package = new Package("PKG1", 10, 100, "OFR003");

        Assert.True(offer.IsApplicable(package));
    }

    [Fact]
    public void IsApplicable_WhenWeightTooLow_ReturnsFalse()
    {
        var offer = new Offer("OFR001", 10, 0, 200, 70, 200);
        var package = new Package("PKG1", 5, 30, "OFR001");

        Assert.False(offer.IsApplicable(package));
    }

    [Fact]
    public void IsApplicable_WhenWeightTooHigh_ReturnsFalse()
    {
        var offer = new Offer("OFR001", 10, 0, 200, 70, 200);
        var package = new Package("PKG1", 250, 30, "OFR001");

        Assert.False(offer.IsApplicable(package));
    }

    [Fact]
    public void IsApplicable_WhenDistanceTooLow_ReturnsFalse()
    {
        var offer = new Offer("OFR002", 7, 50, 150, 100, 250);
        var package = new Package("PKG1", 110, 5, "OFR002");

        Assert.False(offer.IsApplicable(package));
    }

    [Fact]
    public void IsApplicable_WhenDistanceTooHigh_ReturnsFalse()
    {
        var offer = new Offer("OFR002", 7, 50, 150, 100, 250);
        var package = new Package("PKG1", 110, 200, "OFR002");

        Assert.False(offer.IsApplicable(package));
    }

    [Fact]
    public void IsApplicable_AtMinimumBoundaries_ReturnsTrue()
    {
        var offer = new Offer("OFR003", 5, 50, 250, 10, 150);
        var package = new Package("PKG1", 10, 50, "OFR003");

        Assert.True(offer.IsApplicable(package));
    }

    [Fact]
    public void IsApplicable_AtMaximumBoundaries_ReturnsTrue()
    {
        var offer = new Offer("OFR003", 5, 50, 250, 10, 150);
        var package = new Package("PKG1", 150, 249, "OFR003");

        Assert.True(offer.IsApplicable(package));
    }
}
