﻿using INBS.Domain.IRepository;
using INBS.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace INBS.Persistence.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal INBSDbContext context;
        internal DbSet<TEntity> dbSet;

        public IQueryable<TEntity> Query()
        {
            return dbSet.AsQueryable();
        }

        public GenericRepository(INBSDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual TEntity? GetByID(object id)
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
            TEntity? entityToDelete = dbSet.Find(id);
            if(entityToDelete != null) Delete(entityToDelete);
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

        public bool Exists(Expression<Func<TEntity, bool>>? filter)
        {
            return filter == null ? throw new ArgumentNullException(nameof(filter)) : dbSet.Any(filter);
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
        public void UpdateRange(IList<TEntity> obj)
        {
            dbSet.UpdateRange(obj);
        }
        public async Task DeleteAsync(object id)
        {
            TEntity entity = await dbSet.FindAsync(id) ?? throw new Exception();
            dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> obj)
        {
            dbSet.RemoveRange(obj);
        }

        public async Task DeleteAsync(object[] keyValues)
        {
            if (keyValues.Length != 2)
                throw new ArgumentException("Composite key requires two values.");

            TEntity entity = await dbSet.FindAsync(keyValues) ?? throw new Exception();
            dbSet.Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = dbSet;

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
