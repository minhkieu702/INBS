using INBS.Domain.IRepository;
using INBS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Persistence.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal INBSDbContext context;
        internal DbSet<TEntity> dbSet;

        public IQueryable<TEntity> Entities => throw new NotImplementedException();

        public GenericRepository(INBSDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
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


        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity obj)
        {
            await dbSet.AddAsync(obj);
        }

        public void InsertRange(IList<TEntity> obj)
        {
            dbSet.AddRange(obj);
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> obj)
        {
            await dbSet.AddRangeAsync(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
        public Task UpdateAsync(TEntity obj)
        {
            return Task.FromResult(dbSet.Update(obj));
        }
        public async Task UpdateRangeAsync(TEntity obj)
        {
            dbSet.UpdateRange(obj);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(object id)
        {
            TEntity entity = await dbSet.FindAsync(id) ?? throw new Exception();
            dbSet.Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }



        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }

        public async Task<int> CountAsync()
        {
            return await dbSet.CountAsync();
        }
    }

}
