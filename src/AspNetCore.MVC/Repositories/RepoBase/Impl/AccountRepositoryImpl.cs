using AspNetCore.MVC.Data;
using AspNetCore.MVC.Models;
using System.Linq;

namespace AspNetCore.MVC.Repositories.RepoBase.Impl
{
    public class AccountRepositoryImpl : RepositoryBaseImpl<Account>, AccountRepository
    {
        private readonly ETransportationSystemContext _context;
        public AccountRepositoryImpl()
        {

        }

        public AccountRepositoryImpl(ETransportationSystemContext context)
        {
            _context = context;
        }

        public Account FindByUsernameAndPassword(string username, string password)
        {
            using (var _context = new ETransportationSystemContext())
            {
                return _context.Accounts.FirstOrDefault(c => c.Username.Equals(username) && c.Email.Equals(password));
            }
            //return GetAll().FirstOrDefault(c => c.Username.Equals(username) && c.Password.Equals(password));
        }

        // nếu muốn overide thì sửa lại cái này còn nếu không thì xóa đi hoặc để là base.function(entity);
        public override void Add(Account entity) => base.Add(entity);
        public override void Delete(int id) => base.Delete(id);
        public override IQueryable<Account> GetAll() => base.GetAll();
        public override Account GetById(int id) => base.GetById(id);
        public override void Update(Account entity) => base.Update(entity);
    }
}
