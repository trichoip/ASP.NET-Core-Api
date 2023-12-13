using AspNetCore.Client.MVC.Models;

namespace AspNetCore.Client.MVC.Repositories.ServiceBase;

public interface AccountService : ServiceBase<Account>
{
    Account SignIn(string username, string password);
}
