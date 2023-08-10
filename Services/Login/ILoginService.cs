using Microsoft.AspNetCore.Identity;
using RoushUSPS_App.ViewModels.Login;

namespace RoushUSPS_App.Services.Login
{
    public interface ILoginService
    {
        Task<IdentityResult> CreateUserAsync(UserCreateViewModel model);
        Task<SignInResult> LoginAsync(UserLoginViewModel model);
        Task SignOutAsync();

    }
}
