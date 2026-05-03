using AtTheMovies.Behaviors;
using AtTheMovies.Commands.Movies;
using AtTheMovies.Validators.Movies;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;

namespace AtTheMovies.Tests.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_ValidRequest_ShouldCallNext()
    {
        //arrange
        var validators = new List<IValidator<CreateMovieCommand>>
        {
            new CreateMovieCommandValidator()
        };

        var behavior = new ValidationBehavior<CreateMovieCommand, int>(validators);

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
        nextCalled.Should().Be(true);
        result.Should().Be(1);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldThrowValidationException()
    {
        //arrange
        var validators = new List<IValidator<CreateMovieCommand>>
        {
            new CreateMovieCommandValidator()
        };

        var behavior = new ValidationBehavior<CreateMovieCommand, int>(validators);

        var request = new CreateMovieCommand
        {
            Name = "", // Invalid - empty title
            Genre = "Action",
            Description = "Valid description"
        };

        RequestHandlerDelegate<int> next = (context) => Task.FromResult(1);

        //act & assert
        await Assert.ThrowsAsync<ValidationException>(
                 () => behavior.Handle(request, next, CancellationToken.None)
             );
    }

    [Fact]
    public async Task Handle_NoValidators_ShouldCallNextDirectly()
    {
        //arrange
        var validators = new List<IValidator<CreateMovieCommand>>(); // No validators provided

        var behavior = new ValidationBehavior<CreateMovieCommand, int>(validators);

        var request = new CreateMovieCommand
        {
            Name = "", // Invalid - empty title, but no validators to catch this
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
        nextCalled.Should().Be(true);
        result.Should().Be(1);
    }

    [Fact]
    public async Task Handle_MultipleValidatorsWithValidRequest_ShouldRunAll()
    {
        //arrange
        var mockValidator1 = new Mock<IValidator<CreateMovieCommand>>();
        mockValidator1
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateMovieCommand>>(),
                It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var validators = new List<IValidator<CreateMovieCommand>>
        {
            new CreateMovieCommandValidator(),
            mockValidator1.Object
        };

        var behavior = new ValidationBehavior<CreateMovieCommand, int>(validators);

        var request = new CreateMovieCommand
        {
            Name = "Valid Title",
            Genre = "Action",
            Description = "Valid description"
        };

        RequestHandlerDelegate<int> next = (context) => Task.FromResult(1);

        //act
        await behavior.Handle(request, next, CancellationToken.None);

        //assert
        mockValidator1.Verify(v => 
            v.ValidateAsync(
                It.IsAny<ValidationContext<CreateMovieCommand>>(),
                It.IsAny<CancellationToken>()), 
                Times.Once
                );
    }
}