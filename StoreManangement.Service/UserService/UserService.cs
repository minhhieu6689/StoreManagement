using StoreManagement.Data.DTO;
using StoreManagement.Data.Model;
using StoreManagement.Repository.Repository;
using StoreManagement.Repository.UnitOfWork;
using StoreManangement.Service.BaseService;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StoreManangement.Service.UserService
{
    public class UserService : BaseService<User, UserDto>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IGenericRepository<User> _repository => _unitOfWork.UserRepository;

        public async Task<bool> AuthenticateLogin(UserDto user)
        {
            return await _unitOfWork.UserRepository.ValidateLogin(user);
        }
    }
}
