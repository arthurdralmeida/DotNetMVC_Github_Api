using Domain.GithubRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Results
{
    public class GithubRepoWithStringLanguage : GithubRepo
    {
        public GithubRepoWithStringLanguage(Guid id, long gitId, string name, string fullName, string description, string homepage, LanguageEnum language, string url, string htmlUrl, string cloneUrl, string gitUrl, string sshUrl, string svnUrl, string mirrorUrl, string archiveUrl, string nodeId, bool isTemplate, bool @private, bool fork, int forksCount, int stargazersCount, string defaultBranch, int openIssuesCount, DateTime createdAt, DateTime updatedAt, string parent, string source, List<string> topics, Licence license, Owner owner) : base(id, gitId, name, fullName, description, homepage, language, url, htmlUrl, cloneUrl, gitUrl, sshUrl, svnUrl, mirrorUrl, archiveUrl, nodeId, isTemplate, @private, fork, forksCount, stargazersCount, defaultBranch, openIssuesCount, createdAt, updatedAt, parent, source, topics, license, owner)
        {
            Language = language.ToString();
        }

        public new string Language { get; set; }
    }
}
