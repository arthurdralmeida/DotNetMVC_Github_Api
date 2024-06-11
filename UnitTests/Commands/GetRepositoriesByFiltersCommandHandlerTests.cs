using Application.Commands.GetRepositoriesByFilters;
using Application.OutputPorts;
using Moq;

namespace UnitTests.Commands
{
    public class GetRepositoriesByFiltersCommandHandlerTests
    {
        private readonly Mock<IGithubRepoRepository> _githubRepoRepositoryMock;

        public GetRepositoriesByFiltersCommandHandlerTests()
        {
            _githubRepoRepositoryMock = new Mock<IGithubRepoRepository>();
        }

        [Fact]
        [Trait("Commands", "GetRepositoriesByFiltersCommandHandler")]
        public async Task Handle_ShouldNotAllowPageBellowOne()
        {
            // Arrange
            var handler = new GetRepositoriesByFiltersCommandHandler(_githubRepoRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetRepositoriesByFiltersCommand(0,1,""), CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }

        [Fact]
        [Trait("Commands", "GetRepositoriesByFiltersCommandHandler")]
        public async Task Handle_ShouldNotAllowLimitBellowOne()
        {
            // Arrange
            var handler = new GetRepositoriesByFiltersCommandHandler(_githubRepoRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetRepositoriesByFiltersCommand(1, 0, ""), CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }

        [Fact]
        [Trait("Commands", "GetRepositoriesByFiltersCommandHandler")]
        public async Task Handle_ShouldNotAllowLimitAboveOneHundred()
        {
            // Arrange
            var handler = new GetRepositoriesByFiltersCommandHandler(_githubRepoRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetRepositoriesByFiltersCommand(1, 101, ""), CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }

        [Fact]
        [Trait("Commands", "GetRepositoriesByFiltersCommandHandler")]
        public async Task Handle_ShouldNotAllowNotValidLanguage()
        {
            // Arrange
            var handler = new GetRepositoriesByFiltersCommandHandler(_githubRepoRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetRepositoriesByFiltersCommand(1, 1, "NotValidLanguage"), CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }
    }
}
