using CoordinatesServiceWebApi.Dtos;
using CoordinatesServiceWebApi.Domain;
using CoordinatesServiceWebApi.Dtos;

namespace CoordinatesServiceWebApi.Application.Services;

public class CoordinatesService
{
    private readonly Random _random;
    
    public CoordinatesService(Random? random = null) => 
        _random = random ?? Random.Shared;

    public IReadOnlyCollection<CoordinateDto> GenerateCoordinates(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GeoCoordinate.CreateRandom(_random))
            .Select(CoordinateMapper.MapToDto)
            .ToList();
    }
    
    public DistanceDto CalculateTotalDistance(IEnumerable<CoordinateDto> coordinateDtos)
    {
        List<GeoCoordinate> coordinates = coordinateDtos.Select(CoordinateMapper.MapToDomain).ToList();
        Distance totalDistance = new();

        for (int i = 0; i < coordinates.Count - 1; i++)
            totalDistance += GeoCoordinate.Distance(coordinates[i], coordinates[i + 1]);

        return new DistanceDto(totalDistance.InMeters, totalDistance.InMiles);
    }
}