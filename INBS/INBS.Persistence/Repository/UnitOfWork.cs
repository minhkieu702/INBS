using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using INBS.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly INBSDbContext _context;
        private bool disposed = false;

        public UnitOfWork(INBSDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Category
        private IGenericRepository<Category>? _categoryRepository;

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                _categoryRepository ??= new GenericRepository<Category>(_context);
                return _categoryRepository;
            }
        }
        #endregion

        #region CategoryService
        private IGenericRepository<CategoryService>? _categoryServiceRepository;
        public IGenericRepository<CategoryService> CategoryServiceRepository 
        { 
            get
            {
                _categoryServiceRepository ??= new GenericRepository<CategoryService>(_context);
                return _categoryServiceRepository;
            }
        }

        #endregion

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
    }
}
