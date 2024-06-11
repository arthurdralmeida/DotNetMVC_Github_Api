using Octokit;

namespace Application.OutputPorts
{
    public interface IGithubService
    {
        Task<IEnumerable<Repository>> GetRepositoriesOfLanguageAsync(string language);
    }
}
