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

        int Save();
        Task<int> SaveAsync();
        void Dispose();
        Task DisposeAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
