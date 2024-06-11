using Application.Commands.Results;
using Application.OutputPorts;
using Domain.GithubRepo;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;

namespace Repositories
{
    public class GithubRepoRepository(SqlDbContext context): IGithubRepoRepository
    {
        private readonly SqlDbContext _context = context;

        public async Task<GetRepositoriesByFiltersResult> GetAllPaginatedAsync(int page, int limit, string language)
        {
            var totalCount = await _context.Repositories
                                           .Where(r => string.IsNullOrEmpty(language) || r.Language == language)
                                           .CountAsync();

            var result = await _context.Repositories
                                 .Where(r => string.IsNullOrEmpty(language) || r.Language == language)
                                 .Include(r => r.Owner)
                                 .Include(r => r.License)
                                 .OrderBy(r => r.StargazersCount)
                                 .Skip((page - 1) * limit)
                                 .Take(limit)
                                 .ToListAsync();

            return new GetRepositoriesByFiltersResult 
            { 
                Repositories = result.Select(Map),
                TotalCount = totalCount, 
                TotalPages = (int)Math.Ceiling((double)totalCount / limit) 
            };
        }

        public async Task<GithubRepoWithStringLanguage?> GetByIdAsync(Guid id)
        {
            var result = await _context.Repositories
                                 .Include(r => r.Owner)
                                 .Include(r => r.License)
                                 .FirstOrDefaultAsync(r => r.Id == id);
            return result == null ? null : Map(result);
        }
         
        public async Task AddAsync(GithubRepo repository)
        {
            var model = Map(repository);

            if(model.License is not null)
            {
                var existingLicence = await _context.Licences.FirstOrDefaultAsync(l => l.Key == repository.License.Key);
                if (existingLicence != null)
                {
                    _context.Entry(existingLicence).State = EntityState.Unchanged;
                    model.License = existingLicence;
                }
            }

            var existingOwner = await _context.Owners.FirstOrDefaultAsync(o => o.GitId == repository.Owner.GitId);
            if (existingOwner != null)
            {
                _context.Entry(existingOwner).State = EntityState.Unchanged;
                model.Owner = existingOwner;
            }

            await _context.Repositories.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            _context.Owners.RemoveRange(_context.Owners);
            _context.Licences.RemoveRange(_context.Licences);
            _context.Repositories.RemoveRange(_context.Repositories);
            await _context.SaveChangesAsync();
        }

        private static GithubRepoWithStringLanguage Map(GithubRepoModel model)
        {
            return new GithubRepoWithStringLanguage(model.Id, model.GitId, model.Name, model.FullName, model.Description,
                                  model.Homepage, Enum.Parse<LanguageEnum>(model.Language), model.Url,
                                  model.HtmlUrl, model.CloneUrl, model.GitUrl, model.SshUrl, model.SvnUrl,
                                  model.MirrorUrl, model.ArchiveUrl, model.NodeId, model.IsTemplate, model.Private,
                                  model.Fork, model.ForksCount, model.StargazersCount, model.DefaultBranch,
                                  model.OpenIssuesCount, model.CreatedAt, model.UpdatedAt, model.Parent, model.Source,
                                  model.Topics, Map(model.License), Map(model.Owner));
        }

        private static GithubRepoModel Map(GithubRepo model)
        {
            return new GithubRepoModel
            {
                Id = model.Id,
                GitId = model.GitId,
                Name = model.Name,
                FullName = model.FullName,
                Description = model.Description,
                Homepage = model.Homepage,
                Language = model.Language.ToString(),
                Url = model.Url,
                HtmlUrl = model.HtmlUrl,
                CloneUrl = model.CloneUrl,
                GitUrl = model.GitUrl,
                SshUrl = model.SshUrl,
                SvnUrl = model.SvnUrl,
                MirrorUrl = model.MirrorUrl,
                ArchiveUrl = model.ArchiveUrl,
                NodeId = model.NodeId,
                IsTemplate = model.IsTemplate,
                Private = model.Private,
                Fork = model.Fork,
                ForksCount = model.ForksCount,
                StargazersCount = model.StargazersCount,
                DefaultBranch = model.DefaultBranch,
                OpenIssuesCount = model.OpenIssuesCount,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                Parent = model.Parent,
                Source = model.Source,
                Topics = model.Topics,
                License = Map(model.License),
                Owner = Map(model.Owner)
            };
        }

        private static Licence? Map(LicenceModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Licence(model.Id, model.NodeId, model.Key, model.Name, model.SpdxId, model.Url, model.Featured);
        }

        private static LicenceModel? Map(Licence licence)
        {
            if (licence == null)
            {
                return null;
            }
            return new LicenceModel
            {
                Id = licence.Id,
                Key = licence.Key,
                Name = licence.Name,
                SpdxId = licence.SpdxId,
                Url = licence.Url,
                NodeId = licence.NodeId
            };
        }

        private static Owner Map(OwnerModel model)
        {
            return new Owner(model.Id, model.GitId, model.Login, model.AvatarUrl, model.Url, model.HtmlUrl, model.Type, model.CreatedAt);
        }

        private static OwnerModel Map(Owner owner)
        {
            return new OwnerModel
            {
                Login = owner.Login,
                Id = owner.Id,
                GitId = owner.GitId,
                AvatarUrl = owner.AvatarUrl,
                Url = owner.Url,
                HtmlUrl = owner.HtmlUrl,
                Type = owner.Type,
            };
        }
    }
}
