using CoordinatesServiceWebApi.Dtos;
using CoordinatesServiceWebApi.Domain;

namespace CoordinatesServiceWebApi.Application.Services;

/// <summary>
/// Сервис для работы с координатами
/// </summary>
public class CoordinatesService
{
    private readonly Random _random;
    
    public CoordinatesService(Random? random = null) => 
        _random = random ?? Random.Shared;

    /// <summary>
    /// Генерирует коллекцию случайных географических координат
    /// </summary>
    /// <param name="count">Количество генерируемых координат</param>
    /// <returns>Коллекцию сгенерированных координат</returns>
    public IReadOnlyCollection<CoordinateDto> GenerateCoordinates(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GeoCoordinate.CreateRandom(_random))
            .Select(CoordinateMapper.MapToDto)
            .ToList();
    }
    
    /// <summary>
    /// Рассчитывает суммарное расстояние между полученными координатами по формуле гаверсинуса
    /// </summary>
    /// <param name="coordinateDtos">Коллекция географических координат</param>
    /// <returns>Суммарное расстояние между полученными координатамии в метрах и милях</returns>
    public DistanceDto CalculateTotalDistance(IEnumerable<CoordinateDto> coordinateDtos)
    {
        List<GeoCoordinate> coordinates = coordinateDtos.Select(CoordinateMapper.MapToDomain).ToList();
        Distance totalDistance = new();

        for (int i = 0; i < coordinates.Count - 1; i++)
            totalDistance += coordinates[i].DistanceTo(coordinates[i + 1]);

        return new DistanceDto(totalDistance.InMeters, totalDistance.InMiles);
    }
}