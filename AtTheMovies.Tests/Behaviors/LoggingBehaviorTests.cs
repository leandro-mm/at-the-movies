using AtTheMovies.Behaviors;
using AtTheMovies.Commands.Movies;
using Castle.Core.Logging;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace AtTheMovies.Tests.Behaviors;

public class LoggingBehaviorTests
{
    [Fact]
    public async Task Handle_ShouldLogRequestAndResponse()
    {
        //arrange
        var loggerMock = new Mock<ILogger<LoggingBehavior<CreateMovieCommand, int>>>();
        var behavior = new LoggingBehavior<CreateMovieCommand, int>(loggerMock.Object);

        var request = new CreateMovieCommand
        {
            Name = "Valid Title",
            Genre = "Action",
            Description = "Valid description"
        };

        var nextCalled = false;
        RequestHandlerDelegate<int> next = (context) =>
        {
            nextCalled = true;
            return Task.FromResult(1);
        };

        //act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        //assert
        Assert.True(nextCalled);
        result.Should().Be(1);

        loggerMock.VerifyLog(x => x.LogInformation(
           $"Handling {typeof(CreateMovieCommand).Name} with content {request}")
           );

        loggerMock.VerifyLog(x => x.LogInformation(
            It.Is<string>(s => s.Contains($"Handled {typeof(CreateMovieCommand).Name}") && s.Contains("1")))
        );
    }

    [Fact]
    public async Task Handle_WhenNextThrows_ShouldLog()
    {
        //arrange
        var loggerMock = new Mock<ILogger<LoggingBehavior<CreateMovieCommand, int>>>();
        var behavior = new LoggingBehavior<CreateMovieCommand, int>(loggerMock.Object);

        var request = new CreateMovieCommand
        {
            Name = "Valid Title",
            Genre = "Action",
            Description = "Valid description"
        };

        RequestHandlerDelegate<int> next = (context) => throw new InvalidOperationException("Test exception");

        //act
        //var result = await behavior.Handle(request, next, CancellationToken.None);

        //act & assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            behavior.Handle(request, next, CancellationToken.None)
            );

        //log de início foi feito
        loggerMock.VerifyLog(x => x.LogInformation(
           $"Handling {typeof(CreateMovieCommand).Name} with content {request}")
           );       
    }
}