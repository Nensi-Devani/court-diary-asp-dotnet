using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System.Linq;
using System.Data.Entity;
using System;

namespace CourtDiary.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        public IActionResult Index()
        {
            var today = DateTime.Today;

            ViewBag.TotalActiveCases = db.Cases.Count(c => c.status == "Active");
            ViewBag.TotalCases = db.Cases.Count();
            
            // For Entity Framework 6 / SQL Server, we might need EntityFunctions or DbFunctions
            // But since this is likely ASP.NET Core with EF Core, we can use Date property
            ViewBag.TodayHearings = db.Hearings.AsEnumerable().Count(h => h.hearing_date.HasValue && h.hearing_date.Value.Date == today);
            ViewBag.TotalHearings = db.Hearings.Count();
            ViewBag.TodayMeetings = db.Meetings.AsEnumerable().Count(m => m.EventDate.Date == today);
            var pendingAmount = db.Hearings.Where(h => h.payment_status == "Pending" || h.payment_status == "Unpaid").Sum(h => (decimal?)h.fee_amount) ?? 0;
            ViewBag.PendingPayments = $"${pendingAmount:0.00}";

            var recentCases = db.Cases
                .Include(c => c.Case_Parties)
                .Include(c => c.Hearings)
                .OrderByDescending(c => c.created_at)
                .Take(5)
                .ToList();

            return View(recentCases);
        }
    }
}
