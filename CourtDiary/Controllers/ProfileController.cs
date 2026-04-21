using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CourtDiary.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Auth");

            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user == null) return RedirectToAction("Login", "Auth");

            return View(user);
        }

        public IActionResult Edit()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Auth");

            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user == null) return RedirectToAction("Login", "Auth");

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User model)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Auth");

            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user != null)
            {
                user.name = model.name;
                user.office_phone_no = model.office_phone_no;
                user.office_address = model.office_address;
                // Add more fields if needed
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Changepassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Changepassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Auth");

            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user != null && user.password == OldPassword)
            {
                if (NewPassword == ConfirmPassword)
                {
                    user.password = NewPassword;
                    db.SaveChanges();
                    ViewBag.Message = "Password changed successfully!";
                }
                else
                {
                    ViewBag.Error = "Passwords do not match.";
                }
            }
            else
            {
                ViewBag.Error = "Invalid old password.";
            }
            return View();
        }
    }
}
