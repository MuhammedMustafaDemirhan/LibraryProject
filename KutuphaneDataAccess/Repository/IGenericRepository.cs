using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneDataAccess.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Create(TEntity entity);
        Task AddAsync(TEntity entity);
        IQueryable<TEntity> GetAll();
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        TEntity GetById(int id);
        void DeleteRange(List<TEntity> entities);
        IQueryable<TEntity> Queryable();
    }
}
