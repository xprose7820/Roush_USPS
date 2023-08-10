using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoushUSPS_App.Services.Login;
using RoushUSPS_App.ViewModels.Login;

namespace RoushUSPS_App.Controllers
{
    public class LoginController : Controller
    {
        private ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _loginService.LoginAsync(model);
            if (result is null)
            {
                ModelState.AddModelError(string.Empty, "User/Password pair doesn't exist");
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IdentityResult result = await _loginService.CreateUserAsync(model);
            if(result is null)
            {
                ModelState.AddModelError(string.Empty, "Passwords don't match");
                return View(model);
            }
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _loginService.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
