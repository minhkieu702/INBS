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
        private INBSDbContext _context;

        #region Category
        private IGenericRepository<Category> _categoryRepository;

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new GenericRepository<Category>(_context);
                }
                return _categoryRepository;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        await _context.DisposeAsync();
                    }
                }
            }
            disposed = true;
        }

        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
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

        private bool disposed = false;

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

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
    }
}
