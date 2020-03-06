using StoreManagement.Data.DTO;
using StoreManagement.Data.Model;
using StoreManangement.Service.BaseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreManangement.Service.UserService
{
    public interface IUserService : IBaseService<User,UserDto>
    {
        Task<bool> AuthenticateLogin(UserDto user);
    }
}
