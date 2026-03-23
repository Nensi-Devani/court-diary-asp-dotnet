using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace CourtDiary.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            // (optional: save data)
            if (Email == "ndevani894@rku.ac.in")
            {
                HttpContext.Session.SetString("UserRole", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("UserRole", "User");
            }

            HttpContext.Session.SetString("UserEmail", Email);

            return RedirectToAction("Index", "Dashboard");
            //return RedirectToAction("Index", "Dashboard"); // ✅ go to dashboard
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
