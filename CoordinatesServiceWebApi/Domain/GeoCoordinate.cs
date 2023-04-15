using System.Data;
using CoordinatesServiceWebApi.Helpers;

namespace CoordinatesServiceWebApi.Domain;

public readonly record struct GeoCoordinate
{
    public const int EarthRadiusInMetres = 6_371_000;

    public const int MinLatitude = -90;
    public const int MaxLatitude = 90;

    public const int MinLongitude = -180;
    public const int MaxLongitude = 180;

    public double Latitude { get; }
    public double Longitude { get; }


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


    public Distance DistanceTo(GeoCoordinate other) => Distance(this, other);

    public static Distance Distance(GeoCoordinate a, GeoCoordinate b)
    {
        static double ToRadians(double degrees) => degrees * Math.PI / 180;
        
        double latitudeA = ToRadians(a.Latitude);
        double latitudeB = ToRadians(b.Latitude);
        double latitudeDiff = latitudeB - latitudeA;
        double latitudeDiffSin = Math.Sin(latitudeDiff / 2);
        
        double longitudeDiff = ToRadians(b.Longitude - a.Longitude);
        double longitudeDiffSin = Math.Sin(longitudeDiff / 2);
        
        double haversine = latitudeDiffSin * latitudeDiffSin +
                           longitudeDiffSin * longitudeDiffSin * Math.Cos(latitudeA) * Math.Cos(latitudeB);
        
        double distanceInMeters = 2 * EarthRadiusInMetres * Math.Asin(Math.Sqrt(haversine));
        
        return Domain.Distance.FromMeters(distanceInMeters);
    }
    
    
    public static GeoCoordinate CreateRandom(Random? random = null)
    {
        random ??= Random.Shared;

        double latitude = random.NextDoubleInRange(MinLatitude, MaxLatitude);
        double longitude = random.NextDoubleInRange(MinLongitude, MaxLongitude);

        return new GeoCoordinate(latitude, longitude);
    }
}