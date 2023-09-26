using AspNetCore.MVC.Models;
using AspNetCore.MVC.Repositories.RepoBase;
using AspNetCore.MVC.Repositories.RepoBase.Impl;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.MVC.Repositories.ServiceBase.Impl
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
