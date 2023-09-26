using AspNetCore.MVC.Models;

namespace AspNetCore.MVC.Repositories.ServiceBase
{
    public interface AccountService : ServiceBase<Account>
    {
        Account SignIn(string username, string password);
    }
}
