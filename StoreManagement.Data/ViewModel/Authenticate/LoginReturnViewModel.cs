using System;
using System.Collections.Generic;
using System.Text;

namespace StoreManagement.Data.ViewModel.Authenticate
{
    public class LoginReturnViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public string RoleId { get; set; }
    }
}
