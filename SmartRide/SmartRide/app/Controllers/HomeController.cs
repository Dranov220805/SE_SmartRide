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
using Interface;
using System.Security.Claims;

namespace Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomerController _customerController;
        private readonly HomeService _homeService;
        private readonly IRideService _rideService;

        public HomeController(IRideService rideService)
        {
            _rideService = rideService;
            _logger = new LoggerFactory().CreateLogger<HomeController>();
        }

        [Authorize(Roles = "customer")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "customer")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> TrackRide()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var result = await _rideService.GetbookRideByEmailAsync(email);
            return View(result);
        }

        [Authorize(Roles = "customer")]
        public IActionResult BookRide()
        {
            return View();
        }

        public IActionResult Preference()
        {
            return View();
        }

        [Authorize(Roles = "customer")]
        public IActionResult FindRide()
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
