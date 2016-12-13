using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLL
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Items(Func<TEntity, bool> predicate = null);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null);
        void Add(TEntity entity);
        void Remove(Guid id);
        void Update(Guid id);
    }
}
