using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.WebApp.ViewModels
{
    public class CustomerViewModel
    {
        // [Required]
        public string Customerid { get; set; }

        [Required]
        [Display(Name = "First Name:")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Last Name:")]
        public string Lastname { get; set; }

        [Required]    
        [Display( Name = "Phone Number:")]
        [StringLength(10)]
        [RegularExpression("[0-9]*")]
        public string Phonenumber { get; set; }

        [Required]
        [Display (Name = "Email Address:")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password:")]
        [MinLength(9)]
        public string Password { get; set; }

         
        //[Required]
        [Display(Name = "Confirm Password:")]
        [MinLength(9)]
        public string ConfirmPassword { get; set; }

    }
}
