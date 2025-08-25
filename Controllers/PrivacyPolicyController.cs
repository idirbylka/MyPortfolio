using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Controllers
{
    public class PrivacyPolicyController : Controller
    {
        /// Displays Privacy Policy section
        public IActionResult Index()
        {
            return View();
        }
    }
}