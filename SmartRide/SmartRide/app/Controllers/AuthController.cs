using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Interface;
using Services;
using Models;

namespace Controllers
{
    public class LogController : Controller
    {
        private readonly IAuthService _authService;
        public LogController(IAuthService authService)
        {
            _authService = authService;
        }

        [RedirectLoggedInUser]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [RedirectLoggedInUser]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [RedirectLoggedInUser]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var account = await _authService.CheckAccountAsync(email, password);
            if (account != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, account.Role)
        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirect based on role
                if (account.Role == "manager")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return with validation errors
            }

            var account = new RegAccount
            {
                UserName = model.FullName,
                Password = model.Password, // Hash in service
                Role = "User",
                Customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    CustomerName = model.FullName,
                    Phone = model.PhoneNumber,
                    Email = model.EmailAddress
                }
            };

            var createdAccount = await _authService.CreateAccountAsync(account);

            if (createdAccount != null)
            {
                // You can redirect to login or dashboard
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Username already exists.");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Log");
        }
    }
}
