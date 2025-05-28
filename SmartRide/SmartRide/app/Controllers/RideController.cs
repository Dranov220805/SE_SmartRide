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
    public class RideController : Controller
    {
        private readonly IRideService _rideService;
        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost]
        public async Task<IActionResult> BookRide([FromBody] RideBookingRequest ride)
        {
            if (ride == null || string.IsNullOrEmpty(ride.PickupLocation) || string.IsNullOrEmpty(ride.DropoffLocation))
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid parameter"
                });
            }

            var pickup = new Location
            {
                LocationId = Guid.NewGuid(),
                Latitude = ride.PickupLatitude,
                Longitude = ride.PickupLongitude,
                Address = ride.PickupLocation
            };

            var dropoff = new Location
            {
                LocationId = Guid.NewGuid(),
                Latitude = ride.DestinationLatitude,
                Longitude = ride.DestinationLongitude,
                Address = ride.DropoffLocation
            };

            var newRide = new Ride
            {
                RideId = Guid.NewGuid(),
                PickupLocationId = pickup.LocationId,
                DropoffLocationId = dropoff.LocationId,
                Status = "Pending",
                UserEmail = ride.UserEmail,
                PickupDate = ride.PickupDate,
                PickupLocation = pickup,
                DropoffLocation = dropoff
            };

            var result = await _rideService.CreateBookRideAsync(newRide, pickup, dropoff);
            if (result != null)
            {
                string redirectUrl = Url.Action("FindRide", "Home");
                return Json(new
                {
                    success = true,
                    redirectUrl,
                    ride = new
                    {
                        pickupLocation = ride.PickupLocation,
                        dropoffLocation = ride.DropoffLocation
                    }
                });
            }
            return StatusCode(500, "An error occurred while booking the ride.");
        }
    }
}
