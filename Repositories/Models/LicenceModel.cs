using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    [Table("Licence")]
    public class LicenceModel
    {
        [Key]
        public Guid Id { get; set; }
        public string NodeId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string SpdxId { get; set; }
        public string Url { get; set; }
        public bool Featured { get; set; }

        public ICollection<GithubRepoModel> Repositories { get; set; }
    }
}
