using System.ComponentModel.DataAnnotations;
using static CoordinatesServiceWebApi.Domain.GeoCoordinate;

namespace CoordinatesServiceWebApi.Dtos;

public record CoordinateDto
{
    [Range(MinLatitude, MaxLatitude, ErrorMessage = "Значение широты быть в диапазоне от {1} до {2}")]
    public double Latitude { get; init; }

    [Range(MinLongitude, MaxLongitude, ErrorMessage = "Значение долготы должно быть в диапазоне от {1} до {2}")]
    public double Longitude { get; init; }

    public CoordinateDto(double latitude, double longitude) => (Latitude, Longitude) = (latitude, longitude);
}