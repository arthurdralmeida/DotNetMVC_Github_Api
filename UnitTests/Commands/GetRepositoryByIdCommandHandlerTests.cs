using Application.Commands.GetRepositoryById;
using Application.OutputPorts;
using Moq;

namespace UnitTests.Commands
{
    public class GetRepositoryByIdCommandHandlerTests
    {
        private readonly Mock<IGithubRepoRepository> _githubRepoRepositoryMock;

        public GetRepositoryByIdCommandHandlerTests()
        {
            _githubRepoRepositoryMock = new Mock<IGithubRepoRepository>();
        }

        [Fact]
        [Trait("Commands", "GetRepositoryByIdCommandHandler")]
        public async Task Handle_ShouldNotAllowInvalidGuid()
        {
            // Arrange
            var handler = new GetRepositoryByIdCommandHandler(_githubRepoRepositoryMock.Object);


            // Act
            var result = await handler.Handle(new GetRepositoryByIdCommand(Guid.Empty), CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
        }
    }
}
