using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class MeetingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
