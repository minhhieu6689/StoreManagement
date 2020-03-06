using Microsoft.EntityFrameworkCore;
using StoreManagement.Data.Model;
using StoreManangement.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StoreManangement.Service.EFCore
{
    public class UserRepository : EfCoreRepository<User, StoreManagementDbContext>
    {
        private StoreManagementDbContext context;
        public UserRepository(StoreManagementDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<bool> ValidateLogin(User user)
        {
            var result = await context.Users.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));
            if(result != null)
            {
                if (VerifyMd5Hash(user.PasswordHash, result.PasswordHash))
                {
                    return true;
                }
            }
            return false;
        }
        static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
        static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
