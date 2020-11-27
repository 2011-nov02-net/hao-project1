using System;
using System.Collections.Generic;
using System.Text;

namespace StoreLibrary
{
    public class CAdmincredential
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CAdmincredential(string email, string password)
        {
            Email = email;
            Password = password;
        }

    }
}
