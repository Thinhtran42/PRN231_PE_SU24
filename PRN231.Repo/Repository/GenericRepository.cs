using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRN231.Repo.Models;
using PRN231.Repo.ViewModels;

namespace PRN231.Repo.Repository;

 public class GenericRepository<TEntity> where TEntity : class
    {
        internal PRN231_SU23_StudentGroupDBContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(PRN231_SU23_StudentGroupDBContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        // Updated Get method with pagination
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null)  // Optional parameter for pagination (number of records per page)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        
        

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public async virtual Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual bool isExists(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }
        
        public virtual async Task<TEntity> FindUserAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }
    }