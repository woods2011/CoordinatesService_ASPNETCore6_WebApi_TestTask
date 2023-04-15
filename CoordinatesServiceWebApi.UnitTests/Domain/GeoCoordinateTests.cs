using CoordinatesServiceWebApi.Domain;

namespace CoordinatesServiceWebApi.UnitTests.Domain;

[TestFixture]
public class TestGeoCoordinate
{
    private const double Tol = 1e-4;

    [TestCase(0, 0)]
    [TestCase(-90, 180)]
    [TestCase(90, -180)]
    [TestCase(-85.55, -160.77)]
    public void GeoCoordinateCtor_ReturnsValidInstance_WhenCoordinatesAreValid(double latitude, double longitude)
    {
        // Act
        GeoCoordinate geoCoordinate = new GeoCoordinate(latitude, longitude);

        // Assert
        Assert.That(geoCoordinate.Latitude, Is.EqualTo(latitude).Within(Tol));
        Assert.That(geoCoordinate.Longitude, Is.EqualTo(longitude).Within(Tol));
    }


    [TestCase(GeoCoordinate.MinLatitude - 1e-10, 0)]
    [TestCase(GeoCoordinate.MaxLatitude + 1, 0)]
    [TestCase(0, GeoCoordinate.MinLongitude - 1)]
    [TestCase(0, GeoCoordinate.MaxLongitude + 1e+10)]
    public void GeoCoordinateCtor_ThrowsDomainException_WhenCoordinatesAreInvalid(double latitude, double longitude)
    {
        // Act 
        void Act() => _ = new GeoCoordinate(latitude, longitude);

        // Assert
        Assert.Throws<DomainValidationException>(Act);
    }


    [Test]
    public void CreateRandom_ReturnsValidInstances()
    {
        // Act
        void Act() => _ = Enumerable.Range(0, 10).Select(_ => GeoCoordinate.CreateRandom()).ToList();

        // Assert
        Assert.DoesNotThrow(Act);
    }


    [TestCaseSource(nameof(DistanceToTestData))]
    public void DistanceTo_ReturnsCorrectDistanceBetweenTwoPoints(
        GeoCoordinate start,
        GeoCoordinate end,
        Distance expectedDistance)
    {
        // Act
        Distance actualDistance = start.DistanceTo(end);

        // Assert
        Assert.That(actualDistance.InMeters, Is.EqualTo(expectedDistance.InMeters).Within(Tol));
    }

    private static IEnumerable<object[]> DistanceToTestData => new[]
    {
        new object[]
        {
            new GeoCoordinate(59.9343, 30.3351), // start
            new GeoCoordinate(55.7558, 37.6173), // end
            Distance.FromMeters(633020.18217)    // expected distance
        },
        new object[]
        {
            new GeoCoordinate(0, 30.3351), new GeoCoordinate(55.7558, 0), Distance.FromMeters(6776570.47427)
        },
        new object[]
        {
            new GeoCoordinate(0, 0), new GeoCoordinate(0, 0), Distance.FromMeters(0)
        }
    };

    
    [Test]
    [Repeat(50)]
    public void DistanceTo_RespectsCommutativity()
    {
        // Arrange
        GeoCoordinate start = GeoCoordinate.CreateRandom();
        GeoCoordinate end = GeoCoordinate.CreateRandom();

        // Act
        Distance distance1 = GeoCoordinate.Distance(start, end);
        Distance distance2 = GeoCoordinate.Distance(end, start);

        // Assert
        Assert.That(distance1.InMeters, Is.EqualTo(distance2.InMeters).Within(Tol));
    }
}