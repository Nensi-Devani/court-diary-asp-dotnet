
using CourtDiary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace CourtDiary.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly CourtDiaryContext _context;

        public MeetingsController()
        {
            _context = new CourtDiaryContext();
        }

        public IActionResult Index()
        {
            var meetings = _context.Meetings.ToList();
            return View(meetings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                _context.Meetings.Add(meeting);
                _context.SaveChanges();

                var email = HttpContext.Session.GetString("UserEmail");
                if (!string.IsNullOrEmpty(email))
                {
                    var loggedInUser = _context.Users.FirstOrDefault(usr => usr.email == email);
                    if (loggedInUser != null)
                    {
                        _context.Notifications.Add(new Notification
                        {
                            user_id = loggedInUser.user_id,
                            title = "New Meeting Added",
                            message = $"Meeting '{meeting.Title}' was successfully added.|{meeting.Id}",
                            created_at = DateTime.Now,
                            is_read = false
                        });
                        _context.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            return View(meeting);
        }
    }
}