using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreRopa.Models;
using StoreRopa.Models.Vo;
using System.Diagnostics;

namespace StoreRopa.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CurrentUser _currentUser;
        private readonly User _user;

        public HomeController(ILogger<HomeController> logger, CurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
            _user = _currentUser.Builder();
        }

        public IActionResult Index()
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