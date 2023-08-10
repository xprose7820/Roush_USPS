using System.ComponentModel.DataAnnotations;

namespace RoushUSPS_App.ViewModels.Login
{
    public class UserLoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
