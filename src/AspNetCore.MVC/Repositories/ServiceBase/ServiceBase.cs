using System.Collections.Generic;

namespace asp.net_core_empty_5._0.Repositories.ServiceBase
{
    public interface ServiceBase<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
