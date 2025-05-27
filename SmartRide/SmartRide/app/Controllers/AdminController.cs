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
    [Authorize(Roles = "manager")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Logic to retrieve and display admin dashboard data
            return View();
        }
    }
}
