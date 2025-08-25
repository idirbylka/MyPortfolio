using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Controllers
{

    /// Handles imprint-related actions
    public class ImprintController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}