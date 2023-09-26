using System.Collections.Generic;
using System.Linq;
using asp.net_core_empty_5._0.Repositories.RepoBase;
using asp.net_core_empty_5._0.Repositories.RepoBase.Impl;

namespace asp.net_core_empty_5._0.Repositories.ServiceBase.Impl
{
    public class ServiceBaseImpl<T> : ServiceBase<T> where T : class
    {
        private readonly RepositoryBase<T> _repositoryBase;

        public ServiceBaseImpl()
        {
            _repositoryBase = new RepositoryBaseImpl<T>();
        }
        public ServiceBaseImpl(RepositoryBase<T> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public virtual void Add(T entity)
        {
            _repositoryBase.Add(entity);
        }

        public virtual void Delete(int id)
        {
            _repositoryBase.Delete(id);
        }
        public virtual List<T> GetAll()
        {
            return _repositoryBase.GetAll().ToList();
        }
        public virtual T GetById(int id)
        {
            return _repositoryBase.GetById(id);
        }
        public virtual void Update(T entity)
        {
            _repositoryBase.Update(entity);
        }
    }
}
