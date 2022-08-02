using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceAppWeb.Models;

namespace PersonalFinanceAppWeb.Controllers
{
    public class LoginAndRegistrationController : Controller
    {

        private readonly ILogger<LoginAndRegistrationController> _logger;
        private IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginAndRegistrationController(ILogger<LoginAndRegistrationController> logger, IConfiguration configuration, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Registration register = new Registration();
            return View(register);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Register()
        {
            Registration register = new Registration();
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Registration userRegistration)
        {
            var user = new IdentityUser()
            {
                UserName = userRegistration.UserName,
                Email = userRegistration.Email,
                PhoneNumber = userRegistration.PhoneNumber
                

            };

            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (result.Succeeded)
            {

                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Login");
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var item in result.Errors)
                {
                    errors.Add(item.Description.ToString());
                }
                userRegistration.ErrorList = errors;
                return View(userRegistration);
            }

        }

        public async Task<IActionResult> Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login userLogin,string returnURL=null)
        {

            var identityResult = await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, userLogin.RememberMe, false);
            
            if (identityResult.Succeeded)
            {
                if(returnURL ==null || returnURL =="/")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return View();
                }
                
            }

            
            else
            {
                ViewBag.LoginError = "Wrong Email Address or Password. Please try again.";
                return View();
            }


        }

    }
}
