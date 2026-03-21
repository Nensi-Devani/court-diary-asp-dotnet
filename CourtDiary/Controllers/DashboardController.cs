using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
