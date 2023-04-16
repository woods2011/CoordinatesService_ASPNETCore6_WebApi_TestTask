using CoordinatesServiceWebApi.Helpers;

namespace CoordinatesServiceWebApi.UnitTests.Helpers;

[TestFixture]
public class RandomExtensionsTests
{
    private const double Tol = 1e-4;

    [Test]
    public void NextDoubleInRange_ReturnsValueWithinRange_WhenRangeIsValid()
    {
        // Arrange
        Random random = Random.Shared;
        const double minValue = -1e-5;
        const double maxValue = -minValue;
        const int numberOfTests = 50;

        // Act
        List<double> values = Enumerable.Range(0, numberOfTests)
            .Select(_ => random.NextDoubleInRange(minValue, maxValue))
            .ToList();

        // Assert
        Assert.That(values, Has.All.GreaterThanOrEqualTo(minValue).Within(Tol));
        Assert.That(values, Has.All.LessThanOrEqualTo(maxValue).Within(Tol));
    }

    [Test]
    public void NextDoubleInRange_ThrowsArgumentOutOfRangeException_WhenMinValueIsGreaterThanMaxValue()
    {
        // Arrange
        Random? random = Random.Shared;
        const double minValue = 50.0;
        const double maxValue = -50.0;

        // Act
        void Act() => random.NextDoubleInRange(minValue, maxValue);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [TestCase(0)]
    [TestCase(10.01)]
    [TestCase(-5.01)]
    public void NextDoubleInRange_ReturnsSameValue_WhenMinValueAndMaxValueAreEqual(double expectedValue)
    {
        // Arrange
        Random random = Random.Shared;

        // Act
        double value = random.NextDoubleInRange(expectedValue, expectedValue);

        // Assert
        Assert.That(value, Is.EqualTo(expectedValue));
    }
}