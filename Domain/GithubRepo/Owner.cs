namespace Domain.GithubRepo
{
    public class Owner
    {
        public Guid Id { get; private set; }
        public long GitId { get; private set; }
        public string Login { get; private set; }
        public string AvatarUrl { get; private set; }
        public string Url { get; private set; }
        public string HtmlUrl { get;  private set; }
        public string Type { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Owner(Guid id, long gitId, string login, string avatarUrl, string url, string htmlUrl, string type, DateTime createdAt)
        {
            Id = id;
            GitId = gitId;
            Login = login;
            AvatarUrl = avatarUrl;
            Url = url;
            HtmlUrl = htmlUrl;
            Type = type;
            CreatedAt = createdAt;
        }

        public static Owner Create(long gitId, string login, string avatarUrl, string url, string htmlUrl, string type, DateTime createdAt)
        {
            return new Owner(Guid.NewGuid(), gitId, login, avatarUrl, url, htmlUrl, type, createdAt);
        }
    }
}
