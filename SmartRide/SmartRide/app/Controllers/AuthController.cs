using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Interface;
using Services;
using Models;
using Microsoft.AspNetCore.Identity.Data;

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
        public async Task<IActionResult> Login([FromBody] Account request)
        {
            var email = request.Email;
            var password = request.Password;

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

                string redirectUrl = account.Role switch
                {
                    "manager" => Url.Action("Index", "Admin"),
                    "driver" => Url.Action("Index", "Driver"),
                    _ => Url.Action("Index", "Home")
                };
               
                return Json(new { 
                    success = true,
                    redirectUrl,
                    user = new
                    {
                        id = account.AccountId,
                        username = account.UserName,
                        email = account.Email
                    }
                });
            }

            return BadRequest(new { 
                success = false, 
                error = "Invalid login" 
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Account model)
        {
            var account = new Account
            {
                UserName = model.UserName,
                Password = model.Password,
                Phone = model.Phone,
                Email = model.Email,
                Role = "User"
            };

            var createdAccount = await _authService.CreateAccountAsync(account);

            if (createdAccount != null)
            {
                string redirectUrl = Url.Action("Login", "Log");
                return Json(new
                {
                    success = true,
                    redirectUrl,
                    user = new
                    {
                        id = account.AccountId,
                        username = account.UserName,
                        email = account.Email,
                        phone = account.Phone
                    }
                });
            }

            return BadRequest(new { 
                success = false, 
                error = "Invalid register" 
            });
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Log");
        }
    }
}
