using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Changepassword()
        {
            return View();
        }
    }
}
