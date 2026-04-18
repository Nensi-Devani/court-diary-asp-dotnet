
using CourtDiary.Models;
using Microsoft.AspNetCore.Mvc;
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
                return RedirectToAction("Index");
            }
            return View(meeting);
        }
    }
}