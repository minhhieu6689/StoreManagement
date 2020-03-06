using StoreManagement.Data.Model;
using StoreManangement.Data.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreManangement.Service.EFCore
{
    public class EmployeeRepository : EfCoreRepository<Employee, StoreManagementDbContext>
    {

        public EmployeeRepository(StoreManagementDbContext context) : base(context)
        {
           
        }
    }
}
