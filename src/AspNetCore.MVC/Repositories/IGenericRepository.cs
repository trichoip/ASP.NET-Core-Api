using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net_core_empty_5._0.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_empty_5._0.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ETransportationSystemContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ETransportationSystemContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            T entityToDelete = _dbSet.Find(id);
            _dbSet.Remove(entityToDelete);
            _context.SaveChanges();
        }
    }
}