using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using Controllers;
using Repository;
using Services;
using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;
        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }
        [Authorize(Roles = "driver")]
        public async Task<IActionResult> Index()
        {
            // Logic to retrieve and display admin dashboard data
            var result = await _driverService.GetAllRidesAsync();
            return View(result);
        }

        [Authorize(Roles = "driver")]
        public IActionResult ViewRides()
        {
            // Logic to retrieve and display rides assigned to the driver
            return View();
        }

        [Authorize(Roles = "driver")]
        public IActionResult TrackRide()
        {
            // Logic to retrieve and display rides assigned to the driver
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return View();
        }

        [Authorize(Roles = "driver")]
        public IActionResult AcceptRide(int rideId)
        {
            // Logic to accept a ride
            // This would typically involve updating the ride status in the database
            return RedirectToAction("ViewRides");
        }

        [Authorize(Roles = "driver")]
        public IActionResult CompleteRide(int rideId)
        {
            // Logic to mark a ride as completed
            // This would typically involve updating the ride status in the database
            return RedirectToAction("ViewRides");
        }

        [HttpPost]
        public async Task<IActionResult> TakeRide([FromBody] RideAssignmentRequest request)
        {
            var driverEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (request == null || string.IsNullOrEmpty(driverEmail) || string.IsNullOrEmpty(request.UserEmail))
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid parameter"
                });
            }
            // Logic to assign the ride to the driver based on email and rideId
            var result = await _driverService.AssignRideToDriverAsync(driverEmail, request.UserEmail);
            if (result == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { 
                    success = false, 
                    results = result,
                    error = "Failed to assign ride" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] EmailRequest request)
        {
            var email = request.Email;
            //var result = await _driverService.CreateDriverAsync(email);
            // Use request.Email if needed
            string redirectUrl = Url.Action("Index", "Driver");

            // Logic to add driver based on request.Email goes here

            return Json(new { success = true, redirectUrl });
        }

        public class EmailRequest
        {
            public string Email { get; set; }
        }

    }
}
