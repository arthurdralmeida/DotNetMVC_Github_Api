using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    [Table("Owner")]
    public class OwnerModel
    {
        [Key]
        public Guid Id { get; set; }
        public long GitId { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<GithubRepoModel> Repositories { get; set; }
    }
}
