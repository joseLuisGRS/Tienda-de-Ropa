using Microsoft.AspNetCore.Mvc;

namespace StoreRopa.Controllers
{
    public class VentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
