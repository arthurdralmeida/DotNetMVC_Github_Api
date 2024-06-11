using Octokit;

namespace UnitTests.Fixtures
{
    public class GithubServicesFixtures
    {
        public static Repository NewRepository()
        {
            return new Repository("url", "htmlUrl", "cloneUrl", "gitUrl", "sshUrl", "svnUrl", 
                                  "mirrorUrl", "archiveUrl", 12345,"nodeId", NewUser(), "name", 
                                  "fullName", true, "description", "homepage", "CSharp", true, 
                                  true, 0, 0, "defaultBranch", 0, null, DateTimeOffset.Now, 
                                  DateTimeOffset.Now, new RepositoryPermissions(), new Repository(), 
                                  new Repository(), NewLicense(), true, true, true, true, 
                                  true, 0, 1234, true, true, true, true, 0, true, RepositoryVisibility.Public,
                                  [], true, true,true);
        }
        public static User NewUser()
        {
            return new User("avatarUrl","bio", "blog", 0, "company", DateTimeOffset.Now,
            DateTimeOffset.Now,0,"email", 0, 0, true, "htmlUrl",0, 0,
            "location", "login", "name", "nodeId", 0, new Plan(),
            0, 0, 0, "url", new RepositoryPermissions(), true,
            "ldapDistinguishedName", null);
        }
        public static LicenseMetadata NewLicense()
        {
            return new LicenseMetadata("key", "nodeId", "name", "spdxId", "url", true);
        }
    }
}
