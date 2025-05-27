using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using Controllers;
using Repository;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomerController _customerController;
        private readonly HomeService _homeService;

        public HomeController(AccountRepository accountRepository)
        {
            _homeService = new HomeService(accountRepository);
            _logger = new LoggerFactory().CreateLogger<HomeController>();
        }

        public IActionResult Index()
        {
            var accounts = _homeService.GetAllAccounts();
            return View(accounts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TrackRide()
        {
            return View();
        }

        public IActionResult BookRide()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
