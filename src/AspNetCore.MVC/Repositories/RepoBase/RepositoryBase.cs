using System.Linq;

namespace asp.net_core_empty_5._0.Repositories.RepoBase
{
    public interface RepositoryBase<T>
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
