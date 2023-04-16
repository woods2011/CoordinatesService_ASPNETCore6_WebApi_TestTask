using CoordinatesServiceWebApi.Helpers;

namespace CoordinatesServiceWebApi.Domain;

/// <summary>
/// Структура представляющая географическую координату
/// </summary>
public readonly record struct GeoCoordinate
{
    public const int EarthRadiusInMetres = 6_371_000;

    public const int MinLatitude = -90;
    public const int MaxLatitude = 90;

    public const int MinLongitude = -180;
    public const int MaxLongitude = 180;

    public double Latitude { get; }
    public double Longitude { get; }


    /// <summary>
    /// Создает новый экземпляр структуры <see cref="GeoCoordinate"/> с указанной (в градусах) широтой и долготой
    /// </summary>
    /// <param name="latitude">Широта</param>
    /// <param name="longitude">Долгота</param>
    /// <exception cref="DomainValidationException">При недопустимых значениях широты или долготы</exception>
    public GeoCoordinate(double latitude, double longitude) : this()
    {
        if (latitude is < MinLatitude or > MaxLatitude)
            throw new DomainValidationException($"Широта должна быть от {MinLatitude} до {MaxLatitude} градусов.");

        if (longitude is < MinLongitude or > MaxLongitude)
            throw new DomainValidationException(
                $"Долгота должна быть в пределах от {MinLongitude} до {MaxLongitude} градусов.");

        Latitude = latitude;
        Longitude = longitude;
    }
    

    /// <summary>
    /// Рассчитывает дистанцию между двумя географическими координатами по формуле гаверсинуса
    /// </summary>
    /// <param name="second">Вторая географическая координата</param>
    /// <returns>Дистанцию между географическими координатами first и second</returns>
    public Distance DistanceTo(GeoCoordinate second) => Distance(this, second);

    /// <param name="first">Первая географическая координата</param>
    /// <inheritdoc cref="DistanceTo(GeoCoordinate)"/>
    public static Distance Distance(GeoCoordinate first, GeoCoordinate second)
    {
        static double ToRadians(double degrees) => degrees * Math.PI / 180;

        double latitudeA = ToRadians(first.Latitude);
        double latitudeB = ToRadians(second.Latitude);
        double latitudeDiff = latitudeB - latitudeA;
        double latitudeDiffSin = Math.Sin(latitudeDiff / 2);

        double longitudeDiff = ToRadians(second.Longitude - first.Longitude);
        double longitudeDiffSin = Math.Sin(longitudeDiff / 2);

        double haversine = latitudeDiffSin * latitudeDiffSin +
                           longitudeDiffSin * longitudeDiffSin * Math.Cos(latitudeA) * Math.Cos(latitudeB);

        double distanceInMeters = 2 * EarthRadiusInMetres * Math.Asin(Math.Sqrt(haversine));

        return Domain.Distance.FromMeters(distanceInMeters);
    }

    
    /// <summary>
    /// Создает случайную географическую координату
    /// </summary>
    /// <param name="random">Генератор случайных чисел</param>
    /// <returns>Случайную географическую координату</returns>
    public static GeoCoordinate CreateRandom(Random? random = null)
    {
        random ??= Random.Shared;

        double latitude = random.NextDoubleInRange(MinLatitude, MaxLatitude);
        double longitude = random.NextDoubleInRange(MinLongitude, MaxLongitude);

        return new GeoCoordinate(latitude, longitude);
    }
}