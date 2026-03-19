using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string FullName, string Email, string Password, string ConfirmPassword)
        {
            // (optional: save data)

            return RedirectToAction("Index", "Dashboard"); // ✅ go to dashboard
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string FullName, string Email, string Password, string ConfirmPassword)
        {
            // (optional: save data)

            return RedirectToAction("Index", "Dashboard"); // ✅ go to dashboard
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
