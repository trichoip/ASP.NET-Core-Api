using System.Linq;
using asp.net_core_empty_5._0.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_empty_5._0.Repositories.ServiceRopeSamplePe.Repository
{
    public class RepositoryBaseImpl<T> where T : class
    {
        private readonly ETransportationSystemContext _context;
        public RepositoryBaseImpl()
        {
            _context = new ETransportationSystemContext();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
