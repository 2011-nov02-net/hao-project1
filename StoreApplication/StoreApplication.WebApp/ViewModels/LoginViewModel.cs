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
        [StringLength(8)]
        public string Password { get; set; }
    }
}
