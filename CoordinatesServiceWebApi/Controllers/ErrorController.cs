using CoordinatesServiceWebApi.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoordinatesServiceWebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger) =>
        _logger = logger;


    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return exception switch
        {
            null => Problem(),
            DomainValidationException domainValidationEx => HandleDomainValidationException(domainValidationEx),
            DomainException domainEx => HandleDomainException(domainEx),
            _ => HandleRemainExceptions(exception)
        };
    }

    private IActionResult HandleDomainValidationException(DomainValidationException exception)
    {
        _logger.LogInformation(
            exception,
            "Domain validation error occurred while processing user request. {@ErrorMessage}, {@DateTimeUtc}",
            exception.Message, DateTime.UtcNow);

        ValidationProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Domain validation error",
            Detail = exception.Message
        };

        return ValidationProblem(problemDetails);
    }

    private IActionResult HandleDomainException(DomainException exception)
    {
        _logger.LogInformation(
            exception,
            "Domain error occurred while processing user request. {@ErrorMessage}, {@DateTimeUtc}",
            exception.Message, DateTime.UtcNow);

        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Domain error",
            Detail = exception.Message
        };

        return BadRequest(problemDetails);
    }

    private IActionResult HandleRemainExceptions(Exception exception)
    {
        _logger.LogError(
            exception,
            "An error occurred while processing user request. {@ErrorMessage}, {@DateTimeUtc}",
            exception.Message, DateTime.UtcNow);
        
        return Problem();
    }
}