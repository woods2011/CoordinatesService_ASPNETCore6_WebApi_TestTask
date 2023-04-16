using CoordinatesServiceWebApi.Application;
using CoordinatesServiceWebApi.Dtos;
using CoordinatesServiceWebApi.Domain;

namespace CoordinatesServiceWebApi.UnitTests.Application;

[TestFixture]
public class CoordinateMapperTests
{
    private const double Tol = 1e-4;
    
    [Test]
    public void MapToDomain_ShouldMapCoordinateDtoToGeoCoordinate()
    {
        // Arrange
        CoordinateDto coordinateDto = new(50.50, 70.70);

        // Act
        GeoCoordinate coordinateDomain = CoordinateMapper.MapToDomain(coordinateDto);

        // Assert
        Assert.That(coordinateDomain.Latitude, Is.EqualTo(coordinateDto.Latitude).Within(Tol));
        Assert.That(coordinateDomain.Longitude, Is.EqualTo(coordinateDto.Longitude).Within(Tol));
    }

    [Test]
    public void MapToDto_ShouldMapGeoCoordinateToCoordinateDto()
    {
        // Arrange
        GeoCoordinate coordinateDomain = new(50.50, 70.70);

        // Act
        CoordinateDto coordinateDto = CoordinateMapper.MapToDto(coordinateDomain);
        
        // Assert
        Assert.That(coordinateDto.Latitude, Is.EqualTo(coordinateDomain.Latitude).Within(Tol));
        Assert.That(coordinateDto.Longitude, Is.EqualTo(coordinateDomain.Longitude).Within(Tol));
    }
}