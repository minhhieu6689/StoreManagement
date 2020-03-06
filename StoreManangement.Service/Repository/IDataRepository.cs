﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreManangement.Service.Repository
{
    public interface IDataRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
