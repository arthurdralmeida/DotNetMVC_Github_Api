using Microsoft.EntityFrameworkCore;
using Repositories.Models;

namespace Repositories
{
    public class SqlDbContext(DbContextOptions<SqlDbContext> options) :  DbContext(options)
    {
        public DbSet<GithubRepoModel> Repositories { get; set; }
        public DbSet<LicenceModel> Licences { get; set; }
        public DbSet<OwnerModel> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GithubRepoModel>()
                        .HasOne(r => r.Owner)
                        .WithMany(o => o.Repositories)
                        .HasForeignKey(r => r.OwnerId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GithubRepoModel>()
                        .HasOne(r => r.License)
                        .WithMany(o => o.Repositories)
                        .HasForeignKey(r => r.LicenseId)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
