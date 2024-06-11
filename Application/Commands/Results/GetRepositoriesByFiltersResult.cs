using Domain.GithubRepo;

namespace Application.Commands.Results
{
    public class GetRepositoriesByFiltersResult
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<GithubRepoWithStringLanguage> Repositories { get; set; }

    }
}
