using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EMa.WebApp.Controllers
{
    [Route("Account")]
    [Route("")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly DataDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("register")]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("register")]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new AppUser
        //        {
        //            UserName = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            FullName = model.LastName + " " + model.FirstName,
        //            Email = model.Email
        //        };
        //        var result = await userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction(nameof(Login));
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View();
        //}

        [HttpGet]
        //[Route("login")]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[Route("login")]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.PhoneNumber,
                    model.Password,
                    model.RememberMe,
                    false);
                
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }

            }
            return View(model);
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        // This method added for role tutorial
        [HttpGet]
        [Route("access-denied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
