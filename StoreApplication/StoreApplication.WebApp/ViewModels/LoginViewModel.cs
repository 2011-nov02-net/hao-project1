using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        
        public string EmailAddress { get; set; }

        
        [Required]
        [MinLength(9)]
        public string Password { get; set; }
    }
}
