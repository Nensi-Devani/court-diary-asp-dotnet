using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System;
using System.Linq;

namespace CourtDiary.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        public IActionResult Index()
        {
            var today = DateTime.Today;

            ViewBag.TotalLawyers = db.Users.Count(u => u.role == 1);
            ViewBag.TotalCases = db.Cases.Count();
            ViewBag.TotalClients = db.Clients.Count();
            ViewBag.ActiveCases = db.Cases.Count(c => c.status == "Active");
            
            // Assuming EF Core, we can use Enumerable.Count to evaluate Date locally
            ViewBag.TodayHearings = db.Hearings.AsEnumerable().Count(h => h.hearing_date.HasValue && h.hearing_date.Value.Date == today);
            ViewBag.TodayMeetings = db.Meetings.AsEnumerable().Count(m => m.EventDate.Date == today);

            return View();
        }
    }
}
