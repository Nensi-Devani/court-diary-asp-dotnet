using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
