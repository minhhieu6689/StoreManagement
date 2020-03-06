using StoreManagement.Data.Model;
using StoreManagement.Repository.Repository;
using StoreManangement.Data.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        internal StoreManagementDbContext _context;
        public UnitOfWork(StoreManagementDbContext context)
        {
            _context = context;
            InitRepositories();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        private bool _disposed = false;
        public UserRepository UserRepository { get; private set; }

        private void InitRepositories()
        {
            UserRepository = new UserRepository(_context);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
