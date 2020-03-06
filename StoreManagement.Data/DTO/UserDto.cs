using System;
using System.Collections.Generic;
using System.Text;

namespace StoreManagement.Data.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailConfirm { get; set; }

        public int RoleId { get; set; }
    }
}
