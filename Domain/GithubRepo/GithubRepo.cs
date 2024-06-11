namespace Domain.GithubRepo
{
    public class GithubRepo
    {
        public Guid Id { get; private set; }
        public long GitId { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string Description { get; private set; }
        public string Homepage { get; private set; }
        public LanguageEnum Language { get; private set; }
        public string Url { get; private set; }
        public string HtmlUrl { get; private set; }
        public string CloneUrl { get; private set; }
        public string GitUrl { get; private set; }
        public string SshUrl { get; private set; }
        public string SvnUrl { get; private set; }
        public string MirrorUrl { get; private set; }
        public string ArchiveUrl { get; private set; }
        public string NodeId { get; private set; }
        public bool IsTemplate { get; private set; }
        public bool Private { get; private set; }
        public bool Fork { get; private set; }
        public int ForksCount { get; private set; }
        public int StargazersCount { get; private set; }
        public string DefaultBranch { get; private set; }
        public int OpenIssuesCount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string Parent { get; private set; }
        public string Source { get; private set; }
        public List<string> Topics { get; private set; }
        public Licence? License { get; private set; }
        public Owner Owner { get; private set; }

        public GithubRepo(Guid id, long gitId, string name, string fullName, string description, string homepage, 
                          LanguageEnum language, string url, string htmlUrl, string cloneUrl, string gitUrl, 
                          string sshUrl, string svnUrl, string mirrorUrl, string archiveUrl, string nodeId, 
                          bool isTemplate, bool @private, bool fork, int forksCount, int stargazersCount, 
                          string defaultBranch, int openIssuesCount, DateTime createdAt, DateTime updatedAt, 
                          string parent, string source, List<string> topics, Licence license, Owner owner)
        {
            Id = id;
            GitId = gitId;
            Name = name;
            FullName = fullName;
            Description = description;
            Homepage = homepage;
            Language = language;
            Url = url;
            HtmlUrl = htmlUrl;
            CloneUrl = cloneUrl;
            GitUrl = gitUrl;
            SshUrl = sshUrl;
            SvnUrl = svnUrl;
            MirrorUrl = mirrorUrl;
            ArchiveUrl = archiveUrl;
            NodeId = nodeId;
            IsTemplate = isTemplate;
            Private = @private;
            Fork = fork;
            ForksCount = forksCount;
            StargazersCount = stargazersCount;
            DefaultBranch = defaultBranch;
            OpenIssuesCount = openIssuesCount;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Parent = parent;
            Source = source;
            Topics = topics;
            License = license;
            Owner = owner;
        }

        public static GithubRepo Create(long gitId, string name, string fullName, string description, string homepage, 
                                        LanguageEnum language, string url, string htmlUrl, string cloneUrl, string gitUrl, 
                                        string sshUrl, string svnUrl, string mirrorUrl, string archiveUrl, string nodeId,
                                        bool isTemplate, bool @private, bool fork, int forksCount, int stargazersCount, 
                                        string defaultBranch, int openIssuesCount, DateTime createdAt, DateTime updatedAt, 
                                        string parent, string source, List<string> topics, Licence license, Owner owner)
        {
            return new GithubRepo(Guid.NewGuid(), gitId, name, fullName, description, homepage, language, url, htmlUrl, 
                                  cloneUrl, gitUrl, sshUrl, svnUrl, mirrorUrl, archiveUrl, nodeId, isTemplate, @private,
                                  fork, forksCount, stargazersCount, defaultBranch, openIssuesCount, createdAt, updatedAt, 
                                  parent, source, topics, license, owner);
        }
    }
}
