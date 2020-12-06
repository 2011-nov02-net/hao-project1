using System.ComponentModel.DataAnnotations;

namespace StoreApplication.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        //[RegularExpression("")]
        public string Email { get; set; }

        [Required]
        [MinLength(9)]
        public string Password { get; set; }
    }
}
