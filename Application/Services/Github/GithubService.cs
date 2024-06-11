using Application.OutputPorts;
using Octokit;

namespace Application.Services.Github
{
    public class GithubService : IGithubService
    {
        private readonly GitHubClient _client;

        public GithubService()
        {
            _client = new GitHubClient(new ProductHeaderValue("git"));
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesOfLanguageAsync(string language)
        {
            var request = new SearchRepositoriesRequest
            {
                Language = Enum.Parse<Language>(language),
                SortField = RepoSearchSort.Stars,
                Order = SortDirection.Descending
            };

            var result = await _client.Search.SearchRepo(request);
            return result.Items;
        }
    }
}
