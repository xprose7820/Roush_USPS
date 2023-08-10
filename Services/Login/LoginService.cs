using Microsoft.AspNetCore.Identity;
using RoushUSPS_App.Data;
using RoushUSPS_App.Models;
using RoushUSPS_App.ViewModels.Login;

namespace RoushUSPS_App.Services.Login
{
    public class LoginService : ILoginService
    {

        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreateViewModel model)
        {
            if(model.VerifyPassword != model.Password)
            {
                // only one bad case, will add into error bag
                return null;
            }
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.UserName,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return result;
            }
            return result;


        }
        public async Task<SignInResult> LoginAsync(UserLoginViewModel model)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null)
            {
                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);   
                    return result;

                }
            }
            return null;

        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
