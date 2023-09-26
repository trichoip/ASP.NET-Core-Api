using asp.net_core_empty_5._0.Models;

namespace asp.net_core_empty_5._0.Repositories.RepoBase
{
    public interface AccountRepository : RepositoryBase<Account>
    {
        Account FindByUsernameAndPassword(string username, string password);
    }
}
