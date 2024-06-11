using Application.Commands.Results;
using Domain.GithubRepo;

namespace Application.OutputPorts
{
    public interface IGithubRepoRepository
    {
        Task<GetRepositoriesByFiltersResult> GetAllPaginatedAsync(int page, int limit, string language);
        Task<GithubRepoWithStringLanguage?> GetByIdAsync(Guid id);
        Task AddAsync(GithubRepo repository);
        Task DeleteAllAsync();
    }
}
