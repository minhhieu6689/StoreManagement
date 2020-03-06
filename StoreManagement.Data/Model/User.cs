using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreManagement.Data.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(int.MaxValue)]
        public string PasswordHash { get; set; }

        public bool EmailConfirm { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }



    }
}
