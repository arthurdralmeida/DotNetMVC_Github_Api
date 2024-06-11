using Application.Commands.UpdateAllData;
using Application.OutputPorts;
using Domain.GithubRepo;
using Moq;
using Octokit;
using UnitTests.Fixtures;

namespace UnitTests.Commands
{
    public class UpdateAllDataCommandHandlerTests
    {
        private readonly Mock<IGithubRepoRepository> _githubRepoRepositoryMock;
        private readonly Mock<IGithubService> _githubServiceMock;

        public UpdateAllDataCommandHandlerTests()
        {
            _githubRepoRepositoryMock = new Mock<IGithubRepoRepository>();
            _githubServiceMock = new Mock<IGithubService>();
        }

        [Fact]
        [Trait("Commands", "UpdateAllDataCommandHandler")]
        public async Task Handle_ShouldClearDatabases()
        {
            // Arrange
            var handler = new UpdateAllDataCommandHandler(_githubRepoRepositoryMock.Object, _githubServiceMock.Object);

            // Act
            await handler.Handle(new UpdateAllDataCommand(), CancellationToken.None);

            // Assert
            _githubRepoRepositoryMock.Verify(x => x.DeleteAllAsync(), Times.Once);
        }

        [Fact]
        [Trait("Commands", "UpdateAllDataCommandHandler")]
        public async Task Handle_ShouldCallGithubService()
        {
            // Arrange
            var handler = new UpdateAllDataCommandHandler(_githubRepoRepositoryMock.Object, _githubServiceMock.Object);

            // Act
            await handler.Handle(new UpdateAllDataCommand(), CancellationToken.None);

            // Assert
            _githubServiceMock.Verify(x => x.GetRepositoriesOfLanguageAsync("Python"), Times.Once);
            _githubServiceMock.Verify(x => x.GetRepositoriesOfLanguageAsync("Java"), Times.Once);
            _githubServiceMock.Verify(x => x.GetRepositoriesOfLanguageAsync("JavaScript"), Times.Once);
            _githubServiceMock.Verify(x => x.GetRepositoriesOfLanguageAsync("CSharp"), Times.Once);
            _githubServiceMock.Verify(x => x.GetRepositoriesOfLanguageAsync("Ruby"), Times.Once);
        }

        [Fact]
        [Trait("Commands", "UpdateAllDataCommandHandler")]
        public async Task Handle_ShouldSaveRepositories()
        {
            // Arrange
            var handler = new UpdateAllDataCommandHandler(_githubRepoRepositoryMock.Object, _githubServiceMock.Object);
            var repositories = new List<Repository>
            {
                GithubServicesFixtures.NewRepository()
            };

            _githubServiceMock.Setup(x => x.GetRepositoriesOfLanguageAsync(repositories.ElementAt(0).Language)).ReturnsAsync(repositories);

            // Act
            await handler.Handle(new UpdateAllDataCommand(), CancellationToken.None);

            // Assert
            _githubRepoRepositoryMock.Verify(x => x.AddAsync(It.Is<GithubRepo>(r => r.GitId == repositories.ElementAt(0).Id)), Times.Once);
        }
    }
}
