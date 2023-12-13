using System.Linq;

namespace AspNetCore.Client.MVC.Repositories.RepoBase;

public interface RepositoryBase<T>
{
    IQueryable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}
