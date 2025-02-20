﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // query
        IQueryable<TEntity> Entities { get; }

        //non async
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        IEnumerable<TEntity> GetAll();

        TEntity? GetByID(object id);
        void Insert(TEntity entity);
        void InsertRange(IList<TEntity> obj);
        Task InsertRangeAsync(IEnumerable<TEntity> obj);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);

        bool Exists(Expression<Func<TEntity, bool>>? filter);

        void Save();

        // async
        Task<IList<TEntity>> GetAllAsync();
        //Task<IPaginatedList<TEntity>> GetPagging(IQueryable<TEntity> query, int? index, int? pageSize);
        Task<TEntity?> GetByIdAsync(object id);
        Task InsertAsync(TEntity obj);
        Task UpdateAsync(TEntity obj);
        Task DeleteAsync(object id);
        Task SaveAsync();
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        Task<int> CountAsync();
        Task DeleteAsync(object[] keyValues);
        void DeleteRange(IEnumerable<TEntity> obj);
        void UpdateRange(IList<TEntity> obj);
    }

}
