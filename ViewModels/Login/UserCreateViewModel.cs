using System.ComponentModel.DataAnnotations;

namespace RoushUSPS_App.ViewModels.Login
{
    public class UserCreateViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required] 
        public string Password { get; set; }
        [Required]
        public string VerifyPassword { get; set; }
        [Required]
        public string Email { get; set; }

        
        
    }
}   
