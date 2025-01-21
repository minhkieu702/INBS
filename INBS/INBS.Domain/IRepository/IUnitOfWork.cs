using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Category> CategoryRepository { get; }

        public IGenericRepository<CategoryService> CategoryServiceRepository { get; }

        public IGenericRepository<Service> ServiceRepository { get; }

        int Save();
        Task<int> SaveAsync();
        void Dispose();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
