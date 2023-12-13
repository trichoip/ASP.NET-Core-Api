using System.Collections.Generic;

namespace AspNetCore.Client.MVC.Repositories.ServiceBase;

public interface ServiceBase<T>
{
    List<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}
