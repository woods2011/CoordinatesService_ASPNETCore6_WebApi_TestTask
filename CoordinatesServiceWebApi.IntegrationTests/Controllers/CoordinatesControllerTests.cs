using System.Net;
using System.Net.Http.Json;
using CoordinatesServiceWebApi.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CoordinatesServiceWebApi.IntegrationTests.Controllers;

[TestFixture]
public class CoordinatesControllerTests
{
    private readonly HttpClient _client;

    public CoordinatesControllerTests() => _client = new WebApplicationFactory<Program>().CreateClient();

    [Test]
    public async Task Get_ReturnsSuccessAndCoordinates_WhenCountGreaterThanZero([Range(1, 5)] int count)
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync($"/coordinates?count={count}");

        // Assert
        Assert.That(response.IsSuccessStatusCode);
        IReadOnlyCollection<CoordinateDto>? coordinates =
            await response.Content.ReadFromJsonAsync<IReadOnlyCollection<CoordinateDto>>();

        Assert.That(coordinates, Is.Not.Null);
        Assert.That(coordinates!, Has.Count.EqualTo(count));
    }

    [Test]
    public async Task Get_ReturnsBadRequest_WhenCountLessThenOne([Range(-5, 0)] int count)
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync($"/coordinates?count={count}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Get_ReturnsBadRequest_WhenCountNotProvided()
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync($"/coordinates");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }


    [Test]
    public async Task Post_ReturnsSuccessAndCorrectTotalDistance_WhenInputCoordinatesAreValid()
    {
        // Arrange
        CoordinateDto[] coordinates =
        {
            new(60.021158, 30.321135),
            new(60.024157, 30.323133),
            new(60.051155, 30.341132)
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/coordinates", coordinates);

        // Assert
        Assert.That(response.IsSuccessStatusCode);
        DistanceDto? distance = await response.Content.ReadFromJsonAsync<DistanceDto>();

        Assert.That(distance, Is.Not.Null);
        Assert.That(distance!.Metres, Is.EqualTo(3515.53893).Within(1e-3));
        Assert.That(distance!.Miles, Is.EqualTo(2.18445).Within(1e-3));
    }

    [Test]
    public async Task Post_ReturnsBadRequest_WhenOneOfCoordinatesIsInvalid()
    {
        // Arrange
        CoordinateDto invalidCoordinate = new(91, 181);

        CoordinateDto[] coordinates =
        {
            new(60.021158, 30.321135),
            new(60.024157, 30.323133),
            invalidCoordinate,
            new(60.051155, 30.341132)
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/coordinates", coordinates);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    public async Task Post_ReturnsZero_WhenInputCoordinatesContainLessThenTwoElements(int count)
    {
        // Arrange
        CoordinateDto[] coordinates = Enumerable.Repeat(new CoordinateDto(60.021158, 30.321135), count).ToArray();

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/coordinates", coordinates);

        // Assert
        Assert.That(response.IsSuccessStatusCode);
        DistanceDto? distance = await response.Content.ReadFromJsonAsync<DistanceDto>();

        Assert.That(distance, Is.Not.Null);
        Assert.That(distance!.Metres, Is.EqualTo(0).Within(1e-3));
        Assert.That(distance!.Miles, Is.EqualTo(0).Within(1e-3));
    }

    [Test]
    public async Task Post_ReturnsBadRequest_WhenInputCoordinatesAreNotProvided()
    {
        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/coordinates", "");
        var a = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}