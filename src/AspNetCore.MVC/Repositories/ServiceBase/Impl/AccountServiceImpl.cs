using System.Collections.Generic;
using System.Linq;
using asp.net_core_empty_5._0.Models;
using asp.net_core_empty_5._0.Repositories.RepoBase;
using asp.net_core_empty_5._0.Repositories.RepoBase.Impl;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_empty_5._0.Repositories.ServiceBase.Impl
{
    public class AccountServiceImpl : ServiceBaseImpl<Account>, AccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountServiceImpl()
        {
            _accountRepository = new AccountRepositoryImpl();
        }

        public AccountServiceImpl(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account SignIn(string username, string password)
        {

            return _accountRepository.FindByUsernameAndPassword(username, password);
        }
        public override void Add(Account entity) => base.Add(entity);
        public override void Delete(int id) => base.Delete(id);
        public override List<Account> GetAll() => _accountRepository.GetAll().Include(a => a.DrivingLicense).ToList();
        public override Account GetById(int id) => base.GetById(id);
        public override void Update(Account entity) => base.Update(entity);
    }
}
