using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DataBaseContext entities = null;
        private bool disposed = false;

        public UnitOfWork()
        {
            entities = new DataBaseContext();
            entities.Database.Migrate();
        }

        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories.Keys.Contains(typeof(TEntity)))
            {
                return repositories[typeof(TEntity)] as IRepository<TEntity>;
            }
            IRepository<TEntity> repo = new GenericRepository<TEntity>(entities);
            repositories.Add(typeof(TEntity), repo);
            return repo;
        }

        public Tuple<bool, string> SaveChanges()
        {
            try
            {
                entities.SaveChanges();
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }

            return new Tuple<bool, string>(true, "");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
