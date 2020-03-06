using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Repository.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<TEntity> FindByIdAsync(object id);
        void Add(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);

        Task<List<TEntity>> GetAll();

    }
}
