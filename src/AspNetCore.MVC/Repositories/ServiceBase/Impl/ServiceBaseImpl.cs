using AspNetCore.MVC.Repositories.RepoBase;
using AspNetCore.MVC.Repositories.RepoBase.Impl;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.MVC.Repositories.ServiceBase.Impl
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
