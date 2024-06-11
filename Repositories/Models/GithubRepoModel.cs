using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    [Table("GithubRepository")]
    public class GithubRepoModel
    {
        [Key]
        public Guid Id { get; set; }
        public long GitId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Homepage { get; set; }
        public string Language { get; set; }
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string CloneUrl { get; set; }
        public string GitUrl { get; set; }
        public string SshUrl { get; set; }
        public string SvnUrl { get; set; }
        public string MirrorUrl { get; set; }
        public string ArchiveUrl { get; set; }
        public string NodeId { get; set; }
        public bool IsTemplate { get; set; }
        public bool Private { get; set; }
        public bool Fork { get; set; }
        public int ForksCount { get; set; }
        public int StargazersCount { get; set; }
        public string DefaultBranch { get; set; }
        public int OpenIssuesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Parent { get; set; }
        public string Source { get; set; }
        public List<string> Topics { get; set; }

        public Guid? LicenseId { get; set; }
        [ForeignKey("LicenseId")]
        public LicenceModel License { get; set; }

        public Guid OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public OwnerModel Owner { get; set; }
    }
}
