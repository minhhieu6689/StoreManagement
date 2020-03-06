using StoreManagement.Data.Model;
using StoreManagement.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        UserRepository UserRepository { get; }
        Task SaveAsync();
    }
}
