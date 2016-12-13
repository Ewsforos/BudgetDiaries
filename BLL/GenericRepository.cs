using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLL
{
    class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private DataBaseContext entities = null;
        private DbSet<TEntity> _objectSet;

        public GenericRepository(DataBaseContext _entities)
        {
            entities = _entities;
            _objectSet = entities.Set<TEntity>();
        }

        public IEnumerable<TEntity> Items(Func<TEntity, bool> predicate = null)
        {
            if (predicate != null)
            {
                return _objectSet.Where(predicate);
            }

            return _objectSet.AsEnumerable();
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _objectSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        #region CRUD
        public void Add(TEntity entity)
        {
            _objectSet.Add(entity);
        }

        public void Update(Guid id)
        {
            //_objectSet.Attach((TEntity)entity);
        }

        public void Remove(Guid id)
        {
            var entity = _objectSet.FirstOrDefault(x => Equals(x.PK_ID, id)); //(TEntity)Activator.CreateInstance(typeof(TEntity));

            _objectSet.Remove(entity);
        }
        #endregion
    }
}
