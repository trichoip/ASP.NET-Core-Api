using System.Linq;
using AspNetCore.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.MVC.Repositories.RepoBase.Impl
{
    public class RepositoryBaseImpl<T> : RepositoryBase<T> where T : class
    {
        private readonly ETransportationSystemContext _context;
        public RepositoryBaseImpl()
        {
            _context = new ETransportationSystemContext();
        }
        public RepositoryBaseImpl(ETransportationSystemContext context)
        {
            _context = context;
        }
        public virtual void Add(T entity)
        {
            using (var _context = new ETransportationSystemContext())
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(int id)
        {
            using (var _context = new ETransportationSystemContext())
            {
                T entity = GetById(id);
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            // khong nen dong ket noi o day vì IQueryable ma mat kêt nôi thi không tolist duoc
            return _context.Set<T>();
        }

        public virtual T GetById(int id)
        {
            using (var _context = new ETransportationSystemContext())
            {
                return _context.Set<T>().Find((long)id);
            }
        }

        public virtual void Update(T entity)
        {
            using (var _context = new ETransportationSystemContext())
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
