using System.ComponentModel.DataAnnotations;
using CoordinatesServiceWebApi.Application.Services;
using CoordinatesServiceWebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CoordinatesServiceWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CoordinatesController : ControllerBase
{
    private readonly CoordinatesService _coordinatesService;

    public CoordinatesController(CoordinatesService coordinatesService) => _coordinatesService = coordinatesService;

    /// <summary>
    /// Генерирует случайные координаты в виде массива объектов
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /coordinates?count=5
    /// 
    /// </remarks>
    /// <param name="count">Количество генерируемых координат</param>
    /// <returns>Массив сгенерированных координат</returns>
    [HttpGet]
    [SwaggerResponse(200, "Успех")]
    [SwaggerResponse(400, "Неверный ввод")]
    public ActionResult<IReadOnlyCollection<CoordinateDto>> GenerateCoordinates(
        [Required, Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше или равно {1}")]
        int count)
    {
        IReadOnlyCollection<CoordinateDto> coordinates = _coordinatesService.GenerateCoordinates(count);
        return new ActionResult<IReadOnlyCollection<CoordinateDto>>(coordinates);
    }

    /// <summary>
    /// Рассчитывает суммарное расстояние между полученными координатами по формуле гаверсинуса
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     POST /coordinates
    ///     [
    ///         {
    ///             "Latitude": 60.021158,
    ///             "Longitude": 30.321135
    ///         },
    ///         {
    ///             "Latitude": 60.024157,
    ///             "Longitude": 30.323133
    ///         },
    ///         {
    ///             "Latitude": 60.051155,
    ///             "Longitude": 30.341132
    ///         }
    ///     ]
    /// 
    /// </remarks>
    /// <param name="coordinates">Массив координат для расчета дистанции между ними</param>
    /// <returns>Суммарное расстояние между полученными координатамии в метрах и милях</returns>
    [HttpPost]
    [SwaggerResponse(200, "Успех")]
    [SwaggerResponse(400, "Неверный ввод")]
    public ActionResult<DistanceDto> CalculateTotalDistance(IEnumerable<CoordinateDto> coordinates)
        => _coordinatesService.CalculateTotalDistance(coordinates);
}