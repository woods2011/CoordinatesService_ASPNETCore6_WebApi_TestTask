using CoordinatesServiceWebApi.Application;
using CoordinatesServiceWebApi.Application.Services;
using CoordinatesServiceWebApi.Dtos;
using CoordinatesServiceWebApi.Domain;

namespace CoordinatesServiceWebApi.UnitTests.Application;

[TestFixture]
public class CoordinatesServiceTests
{
    private const double Tol = 1e-4;
    private readonly CoordinatesService _sut = new();

    [Test]
    public void GenerateCoordinates_ReturnsCorrectCount([Range(0, 5)] int count)
    {
        // Act
        IReadOnlyCollection<CoordinateDto> coordinates = _sut.GenerateCoordinates(count);

        // Assert
        Assert.That(coordinates, Has.Count.EqualTo(count));
    }
    

    [Test]
    public void GenerateCoordinates_ReturnsValidValues([Range(1, 10)] int count)
    {
        // Act
        void Act() => _sut.GenerateCoordinates(count);

        // Assert
        Assert.DoesNotThrow(Act);
    }
    
    
    [Test]
    public void CalculateTotalDistance_ReturnsZero_WhenLessThenTwoCoordinatesProvided()
    {
        // Arrange
        IEnumerable<CoordinateDto> coordinateDtos = Enumerable.Empty<CoordinateDto>();

        // Act
        DistanceDto distance = _sut.CalculateTotalDistance(coordinateDtos);

        // Assert
        Assert.That(distance.Metres, Is.EqualTo(0.0).Within(Tol));
        Assert.That(distance.Miles, Is.EqualTo(0.0).Within(Tol));
    }


    [Test]
    public void CalculateTotalDistance_ReturnsZero_WhenOnlyOneCoordinateProvided()
    {
        // Arrange
        CoordinateDto[] coordinateDtos = new CoordinateDto[] { new(0.0, 0.0) };

        // Act
        DistanceDto distance = _sut.CalculateTotalDistance(coordinateDtos);

        // Assert
        Assert.That(distance.Metres, Is.EqualTo(0.0).Within(Tol));
        Assert.That(distance.Miles, Is.EqualTo(0.0).Within(Tol));
    }

    
    [Test]
    [Repeat(30)]
    public void CalculateTotalDistance_ReturnsSameValue_WhenReverseOrder()
    {
        // Arrange
        CoordinateDto[] coordinateDtos = Enumerable.Range(0, 5)
            .Select(_ => CoordinateMapper.MapToDto(GeoCoordinate.CreateRandom()))
            .ToArray();

        IEnumerable<CoordinateDto> reversedCoordinateDtos = coordinateDtos.Reverse();
        DistanceDto expectedDistance = _sut.CalculateTotalDistance(coordinateDtos);

        // Act
        DistanceDto distanceForReversed = _sut.CalculateTotalDistance(reversedCoordinateDtos);

        // Assert
        Assert.That(distanceForReversed.Metres, Is.EqualTo(expectedDistance.Metres).Within(Tol));
    }
    
    
    [TestCaseSource(nameof(TestData))]
    public void CalculateTotalDistance_ReturnsCorrectDistance_WhenMoreThenOneCoordinatesProvided(
        CoordinateDto[] coordinateDtos,
        Distance expectedDistance)
    {
        // Act
        DistanceDto actualDistance = _sut.CalculateTotalDistance(coordinateDtos);

        // Assert
        Assert.That(actualDistance.Metres, Is.EqualTo(expectedDistance.InMeters).Within(Tol));
        Assert.That(actualDistance.Miles, Is.EqualTo(expectedDistance.InMiles).Within(Tol));
    }

    private static IEnumerable<object[]> TestData => new[]
    {
        new object[]
        {
            new CoordinateDto[] { new(59.9343, 30.3351), new(55.7558, 37.6173) }, // coordinates
            Distance.FromMeters(633020.18217)                                     // expected distance
        },
        new object[]
        {
            new CoordinateDto[] { new(0, 30.3351), new(55.7558, 0) },
            Distance.FromMeters(6776570.47427)
        },
        new object[]
        {
            new CoordinateDto[] { new(0, 0), new(0, 0) },
            Distance.FromMeters(0)
        },
        new object[]
        {
            new CoordinateDto[] { new(0, 0), new(0, 0), new(0, 0), new(0, 0) },
            Distance.FromMeters(0)
        },
        new object[]
        {
            new CoordinateDto[] { new(59.9343, 30.3351), new(55.7558, 37.6173), new(59.9343, 30.3351) },
            Distance.FromMeters(2 * 633020.18217)
        },
        new object[]
        {
            new CoordinateDto[] { new(0, 30.3351), new(55.7558, 0), new(0, 30.3351), new(55.7558, 0) },
            Distance.FromMeters(3 * 6776570.47427)
        }
    };
}