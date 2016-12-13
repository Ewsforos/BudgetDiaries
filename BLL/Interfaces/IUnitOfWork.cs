using DAL.Entities;
using System;

namespace BLL
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Tuple<bool, string> SaveChanges();
    }
}
