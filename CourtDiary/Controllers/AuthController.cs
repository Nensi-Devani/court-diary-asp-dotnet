using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
