using Application.OutputPorts;
using Domain.GithubRepo;
using ErrorOr;
using MediatR;
using Octokit;

namespace Application.Commands.UpdateAllData
{
    public record UpdateAllDataCommand() : IRequest<ErrorOr<Success>>;
    public class UpdateAllDataCommandHandler(IGithubRepoRepository githubRepoRepository,
                                             IGithubService githubService) : IRequestHandler<UpdateAllDataCommand, ErrorOr<Success>>
    {
        private readonly IGithubRepoRepository _githubRepoRepository = githubRepoRepository;
        private readonly IGithubService _githubService = githubService;

        public async Task<ErrorOr<Success>> Handle(UpdateAllDataCommand request, CancellationToken cancellationToken)
        {
            await ClearDatabases();

            var languages = Enum.GetNames(typeof(LanguageEnum));
            foreach (var language in languages)
            {
                var repositories = await GetGithubReposOfALanguage(language);
                
                foreach (var repository in repositories)
                {
                    await _githubRepoRepository.AddAsync(repository);
                }
            }

            return new Success();
        }

        private async Task ClearDatabases()
        {
            await _githubRepoRepository.DeleteAllAsync();
        }

        private async Task<IEnumerable<GithubRepo>> GetGithubReposOfALanguage(string language)
        {
            return (await _githubService.GetRepositoriesOfLanguageAsync(language)).Select(ConvertToDomain);
        }

        private static GithubRepo ConvertToDomain(Repository repository)
        {
            var licence = (repository.License is null) ?
                            null :
                            Licence.Create(repository.License.NodeId, repository.License.Key, repository.License.Name, repository.License.SpdxId,
                                           repository.License.Url ?? "", repository.License.Featured);

            var owner = Owner.Create(repository.Owner.Id, repository.Owner.Login, repository.Owner.AvatarUrl ?? "", repository.Owner.Url ?? "",
                                     repository.Owner.HtmlUrl ?? "", repository.Owner.Type.ToString(), repository.Owner.CreatedAt.DateTime);
            
            var language = (repository.Language == "C#") ? 
                            "CSharp" : 
                            repository.Language;

            return GithubRepo.Create(repository.Id, repository.Name, repository.FullName, repository.Description ?? "", repository.Homepage ?? "",
                                     Enum.Parse<LanguageEnum>(language), repository.Url ?? "", repository.HtmlUrl ?? "", repository.CloneUrl ?? "", 
                                     repository.GitUrl ?? "", repository.SshUrl ?? "", repository.SvnUrl ?? "", repository.MirrorUrl ?? "",
                                     repository.ArchiveUrl ?? "", repository.NodeId, repository.IsTemplate, repository.Private, repository.Fork,
                                     repository.ForksCount, repository.StargazersCount, repository.DefaultBranch, repository.OpenIssuesCount,
                                     repository.CreatedAt.DateTime, repository.UpdatedAt.DateTime, repository.Parent?.FullName ?? "",
                                     repository.Source?.FullName ?? "", repository.Topics.ToList(), licence, owner);
        }

    }
}
