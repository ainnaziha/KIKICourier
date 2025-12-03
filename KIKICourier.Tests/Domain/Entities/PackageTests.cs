using KIKICourier.Domain.Entities;

namespace KIKICourier.Tests.Domain.Entities;

public class PackageTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesPackage()
    {
        var package = new Package("PKG1", 50, 30, "OFR001");

        Assert.Equal("PKG1", package.Id);
        Assert.Equal(50, package.WeightKg);
        Assert.Equal(30, package.DistanceKm);
        Assert.Equal("OFR001", package.OfferCode);
    }

    [Fact]
    public void Constructor_WithNAOfferCode_SetsOfferCodeToNull()
    {
        var package = new Package("PKG1", 50, 30, "NA");

        Assert.Null(package.OfferCode);
    }

    [Fact]
    public void Constructor_WithEmptyOfferCode_SetsOfferCodeToNull()
    {
        var package = new Package("PKG1", 50, 30, "");

        Assert.Null(package.OfferCode);
    }

    [Fact]
    public void Constructor_WithNullOfferCode_SetsOfferCodeToNull()
    {
        var package = new Package("PKG1", 50, 30, null);

        Assert.Null(package.OfferCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidId_ThrowsArgumentException(string invalidId)
    {
        Assert.Throws<ArgumentException>(() => new Package(invalidId, 50, 30, "OFR001"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Constructor_WithInvalidWeight_ThrowsArgumentException(double invalidWeight)
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG1", invalidWeight, 30, "OFR001"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Constructor_WithInvalidDistance_ThrowsArgumentException(double invalidDistance)
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG1", 50, invalidDistance, "OFR001"));
    }
}
