using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.Data.DTO;
using StoreManagement.Data.Model;
using StoreManagement.Data.ViewModel.Authenticate;
using StoreManagement.Infrastructure;
using StoreManangement.Data.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Repository.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        private StoreManagementDbContext _context;
       
        public UserRepository(StoreManagementDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> ValidateLogin(UserDto user)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));
            if (result != null)
            {
                if (Helper.VerifyMd5Hash(user.PasswordHash, result.PasswordHash))
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
