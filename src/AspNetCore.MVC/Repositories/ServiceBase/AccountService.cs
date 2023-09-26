using asp.net_core_empty_5._0.Models;

namespace asp.net_core_empty_5._0.Repositories.ServiceBase
{
    public interface AccountService : ServiceBase<Account>
    {
        Account SignIn(string username, string password);
    }
}
