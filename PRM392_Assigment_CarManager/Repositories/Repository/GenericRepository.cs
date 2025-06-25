using Microsoft.EntityFrameworkCore;
using Repositories.Interface;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly CarManagementContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(CarManagementContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task Create(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyn(Expression<Func<TEntity, bool>> fillter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? currentPage = null, int? pagesize = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            if (fillter != null)
            {
                query = query.Where(fillter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (currentPage.HasValue && pagesize.HasValue)
            {
                query = query.Skip((currentPage.Value - 1) * pagesize.Value).Take(pagesize.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
