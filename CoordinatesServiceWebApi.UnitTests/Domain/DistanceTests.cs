using CoordinatesServiceWebApi.Domain;

namespace CoordinatesServiceWebApi.UnitTests.Domain;

[TestFixture]
public class DistanceTests
{
    private const double Tol = 1e-4;

    [TestCase(3218.688, 2.0)]
    [TestCase(1609.344, 1.0)]
    [TestCase(804.672, 0.5)]
    public void InMiles_ReturnsCorrectResultInMiles(double inMeters, double expectedInMiles)
    {
        // Arrange
        Distance distance = Distance.FromMeters(inMeters);

        // Act
        double inMiles = distance.InMiles;

        // Assert
        Assert.That(inMiles, Is.EqualTo(expectedInMiles).Within(Tol));
    }


    [TestCase(1000.0)]
    [TestCase(2000.0)]
    [TestCase(0.0)]
    public void InMiters_ReturnsCorrectResultInMiters(double expectedInMeters)
    {
        // Arrange
        Distance distance = Distance.FromMeters(expectedInMeters);

        // Act
        double inMiters = distance.InMeters;

        // Assert
        Assert.That(inMiters, Is.EqualTo(expectedInMeters).Within(Tol));
    }


    [TestCase(1000.0, 2000.0)]
    [TestCase(0.0, 0.0)]
    [TestCase(1000.0, 0.0)]
    public void PlusOperator_ReturnsCorrectSumOfDistances(double inMeters1, double inMeters2)
    {
        // Arrange
        Distance distance1 = Distance.FromMeters(inMeters1);
        Distance distance2 = Distance.FromMeters(inMeters2);
        Distance expectedSum = Distance.FromMeters(inMeters1 + inMeters2);

        // Act
        Distance sum = distance1 + distance2;

        // Assert
        Assert.That(sum.InMeters, Is.EqualTo(expectedSum.InMeters).Within(Tol));
        Assert.That(sum.InMiles, Is.EqualTo(expectedSum.InMiles).Within(Tol));
    }
}