using CoordinatesServiceWebApi.Dtos;
using CoordinatesServiceWebApi.Domain;

namespace CoordinatesServiceWebApi.Application;

public static class CoordinateMapper
{
    public static GeoCoordinate MapToDomain(CoordinateDto coordinateDto) =>
        new(coordinateDto.Latitude, coordinateDto.Longitude);

    public static CoordinateDto MapToDto(GeoCoordinate geoCoordinate) => 
        new(geoCoordinate.Latitude, geoCoordinate.Longitude);
}