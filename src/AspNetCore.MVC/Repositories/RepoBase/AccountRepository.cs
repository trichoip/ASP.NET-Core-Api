using AspNetCore.MVC.Models;

namespace AspNetCore.MVC.Repositories.RepoBase
{
    public interface AccountRepository : RepositoryBase<Account>
    {
        Account FindByUsernameAndPassword(string username, string password);
    }
}
