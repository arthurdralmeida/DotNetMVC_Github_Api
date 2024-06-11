namespace Domain.GithubRepo
{
    public class Licence
    {
        public Guid Id { get; private set; }
        public string NodeId { get;  private set; }
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string SpdxId { get; private set; }
        public string Url { get; private set; }
        public bool Featured { get; private set; }

        public Licence(Guid id, string nodeId, string key, string name, string spdxId, string url, bool featured)
        {
            Id = id;
            NodeId = nodeId;
            Key = key;
            Name = name;
            SpdxId = spdxId;
            Url = url;
            Featured = featured;
        }

        public static Licence Create(string nodeId, string key, string name, string spdxId, string url, bool featured)
        {
            return new Licence(Guid.NewGuid(), nodeId, key, name, spdxId, url, featured);
        }
    }
}
