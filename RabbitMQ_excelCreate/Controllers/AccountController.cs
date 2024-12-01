using Microsoft.AspNetCore.Mvc;

namespace RabbitMQ_excelCreate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if (hasUser == null) 
                    return View();

            var resultSign = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);
            if (!resultSign.Succeeded)
            {
                return View();
            }
            return RedirectToAction("Index", "Product");
        }
    }
}
