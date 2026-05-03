using System.Net.Http.Json;
using FluentAssertions;
using System.Diagnostics;
using System.Net;
using AtTheMovies.Validators.Movies;
using AtTheMovies.Commands.Movies;

namespace AtTheMovies.Tests.Integration.Movies;

public class MovieEndpointsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public MovieEndpointsIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    [Fact]
    public async Task Endpoint_Should_Be_Reachable()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/movies");
        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostMovie_ValidMovie_ReturnsCreated()
    {
        //Arrange
        HttpClient? httpClient = _factory.CreateClient();
        var command = new
        {
            Name = "Test Movie",
            Genre = "Action",
            Description = "A test movie description"
        };

        //Act
        var response = await httpClient.PostAsJsonAsync("/movies", command);

        // For debugging - read error content
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Error: {errorContent}");
        }

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }
    [Fact]
    public void Validator_InvalidCommand_ShouldHaveValidationError()
    {
        // Arrange
        var validator = new CreateMovieCommandValidator();
        var command = new CreateMovieCommand
        {
            Name = "", // Invalid
            Genre = "Action",
            Description = "Test"
        };

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }
    [Fact]
    public async Task PostMovie_InvalidMovie_ReturnsBadRequest()
    {
        //Arrange
        HttpClient? httpClient = _factory.CreateClient();
        var command = new
        {
            Name = "", // Invalid: Name is required
            Genre = "Action",
            Description = "A test movie description"
        };

        //Act
        var response = await httpClient.PostAsJsonAsync("/movies", command);

        // Read the error content for debugging
        var errorContent = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"Status Code: {response.StatusCode}");
        Debug.WriteLine($"Error Content: {errorContent}");

        // For 500 errors, try to get more details
        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            // If using app.UseDeveloperExceptionPage() in development, you might get HTML
            // Try to see if there's an exception message
            Debug.WriteLine($"Response Headers: {string.Join(", ", response.Headers)}");
        }

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);        
    }

    [Fact]
    public async Task GetMovies_ReturnsOkResult()
    {
        //Arrange
        HttpClient? httpClient = _factory.CreateClient();        

        //Act
        var response = await httpClient.GetAsync("/movies?Page=1&PageSize=1");

        // Read the error content for debugging
        var errorContent = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"Status Code: {response.StatusCode}");
        Debug.WriteLine($"Error Content: {errorContent}");

        // For 500 errors, try to get more details
        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            // If using app.UseDeveloperExceptionPage() in development, you might get HTML
            // Try to see if there's an exception message
            Debug.WriteLine($"Response Headers: {string.Join(", ", response.Headers)}");
        }

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);        
    }
}