using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace CourtDiary.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        public IActionResult Read(int id)
        {
            var note = db.Notifications.FirstOrDefault(n => n.notification_id == id);
            if (note != null)
            {
                bool isMeeting = note.title == "New Meeting Added";
                bool deleteIt = true;

                if (isMeeting)
                {
                    string msg = note.message;
                    if (msg.Contains("|"))
                    {
                        var parts = msg.Split('|');
                        if (int.TryParse(parts[parts.Length - 1], out int mId))
                        {
                            var meeting = db.Meetings.FirstOrDefault(m => m.Id == mId);
                            // If meeting is found and its date is in the future, do not delete/mark as read
                            if (meeting != null && meeting.EventDate >= DateTime.Now)
                            {
                                deleteIt = false;
                            }
                        }
                    }
                    else
                    {
                        // Fallback logic for old notifications without ID in message
                        string prefix = "Meeting '";
                        string suffix = "' was successfully added.";
                        if (msg.StartsWith(prefix) && msg.EndsWith(suffix))
                        {
                            string meetingTitle = msg.Substring(prefix.Length, msg.Length - prefix.Length - suffix.Length);
                            var meeting = db.Meetings.FirstOrDefault(m => m.Title == meetingTitle);
                            if (meeting != null && meeting.EventDate >= DateTime.Now)
                            {
                                deleteIt = false;
                            }
                        }
                    }
                }

                if (deleteIt)
                {
                    db.Notifications.Remove(note);
                }
                else
                {
                    note.is_read = true;
                }
                
                db.SaveChanges();

                return View(note);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Index()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Auth");

            var user = db.Users.FirstOrDefault(u => u.email == userEmail);
            if (user == null) return RedirectToAction("Login", "Auth");

            // All unread notifications for the user
            var notifications = db.Notifications.Where(n => n.user_id == user.user_id && n.is_read == false)
                                                .OrderByDescending(n => n.created_at)
                                                .ToList();

            return View(notifications);
        }
    }
}
