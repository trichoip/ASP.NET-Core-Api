using asp.net_core_empty_5._0.Models;

namespace asp.net_core_empty_5._0.Repositories
{
    public interface IAccountRepository2 : IGenericRepository<Account>
    {
        Account login(string username, string password);
    }

    public class AccountRepository2 : GenericRepository<Account>, IAccountRepository2
    {
        public AccountRepository2(ETransportationSystemContext context) : base(context)
        {
        }

        public Account login(string username, string password) => throw new System.NotImplementedException();
    }
}
