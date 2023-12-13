using AspNetCore.Client.MVC.Models;

namespace AspNetCore.Client.MVC.Repositories.RepoBase;

public interface AccountRepository : RepositoryBase<Account>
{
    Account FindByUsernameAndPassword(string username, string password);
}
