using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System;
using System.Collections.Generic;

namespace CourtDiary.Controllers.Admin
{
    [Route("Admin/[controller]/[action]/{id?}")]
    public class CasesController : Controller
    {
        private readonly CourtDiaryContext db = new CourtDiaryContext();

        public IActionResult Index()
        {
            var cases = db.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).ToList();
            ViewBag.Lawyers = db.Users.ToList();
            return View("~/Views/Admin/Cases/Index.cshtml", cases);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Lawyers = db.Users.ToList();
            ViewBag.Clients = db.Clients.ToList();
            return View("~/Views/Admin/Cases/Create.cshtml");
        }

        [HttpPost]
        public IActionResult Create(Case caseObj, int? client_id, int? user_id, List<IFormFile> uploadedFiles, UploadCategory docCategory,
            string oppPartyName, string oppPartyNotes, IFormFile oppPartyAvatarFile,
            string oppLawyerName, string oppLawyerNotes, IFormFile oppLawyerAvatarFile,
            DateTime? firstHearingDate, DateTime? nextHearingDate, DateTime? previousHearingDate)
        {
            caseObj.client_id = client_id;
            caseObj.user_id = user_id ?? 1;

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                db.Cases.Add(caseObj);
                db.SaveChanges();

                if (uploadedFiles != null && uploadedFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in uploadedFiles)
                    {
                        if (file.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            db.Uploads.Add(new Upload
                            {
                                case_id = caseObj.case_id,
                                upload_url = "/uploads/" + uniqueFileName,
                                upload_category = docCategory,
                                created_at = DateTime.Now
                            });
                        }
                    }
                }

                if (!string.IsNullOrEmpty(oppPartyName))
                {
                    string oppAvatar = "/images/user.jpg";
                    if (oppPartyAvatarFile != null && oppPartyAvatarFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(oppPartyAvatarFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create)) { oppPartyAvatarFile.CopyTo(fileStream); }
                        oppAvatar = "/uploads/" + uniqueFileName;
                    }

                    db.Case_Parties.Add(new Case_Party
                    {
                        case_id = caseObj.case_id,
                        name = oppPartyName,
                        type = "Opposite Party",
                        notes = oppPartyNotes,
                        avatar = oppAvatar
                    });
                }

                if (!string.IsNullOrEmpty(oppLawyerName))
                {
                    string lwyAvatar = "/images/user.jpg";
                    if (oppLawyerAvatarFile != null && oppLawyerAvatarFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(oppLawyerAvatarFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create)) { oppLawyerAvatarFile.CopyTo(fileStream); }
                        lwyAvatar = "/uploads/" + uniqueFileName;
                    }

                    db.Case_Parties.Add(new Case_Party
                    {
                        case_id = caseObj.case_id,
                        name = oppLawyerName,
                        type = "Opposite Party Lawyer",
                        notes = oppLawyerNotes,
                        avatar = lwyAvatar
                    });
                }

                if (firstHearingDate.HasValue || nextHearingDate.HasValue || previousHearingDate.HasValue)
                {
                    db.Hearings.Add(new Hearing
                    {
                        case_id = caseObj.case_id,
                        hearing_date = firstHearingDate,
                        next_hearing_date = nextHearingDate,
                        notes = "Initial hearing dates",
                        status = "Scheduled"
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Clients = db.Clients.ToList();
            return View("~/Views/Admin/Cases/Create.cshtml", caseObj);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var caseObj = db.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).FirstOrDefault(c => c.case_id == id);
            if (caseObj == null) return NotFound();

            ViewBag.Lawyers = db.Users.ToList();
            ViewBag.Clients = db.Clients.ToList();
            return View("~/Views/Admin/Cases/Edit.cshtml", caseObj);
        }

        [HttpPost]
        public IActionResult Edit(Case caseObj, int? client_id, int? user_id)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                var existingCase = db.Cases.Find(caseObj.case_id);
                if (existingCase == null) return NotFound();

                existingCase.case_no = caseObj.case_no;
                existingCase.title = caseObj.title;
                existingCase.status = caseObj.status;
                existingCase.description = caseObj.description;
                existingCase.hearing_notes = caseObj.hearing_notes;
                existingCase.previous_hearing_summary = caseObj.previous_hearing_summary;
                existingCase.location = caseObj.location;
                existingCase.client_id = client_id;
                if (user_id.HasValue && user_id.Value > 0)
                    existingCase.user_id = user_id.Value;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Lawyers = db.Users.ToList();
            ViewBag.Clients = db.Clients.ToList();
            return View("~/Views/Admin/Cases/Edit.cshtml", caseObj);
        }

        [HttpGet]
        public IActionResult Viewdetails(int id)
        {
            var caseObj = db.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).FirstOrDefault(c => c.case_id == id);
            if (caseObj == null) return NotFound();
            ViewBag.Lawyers = db.Users.ToList();
            return View("~/Views/Admin/Cases/Viewdetails.cshtml", caseObj);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var caseObj = db.Cases.Find(id);
            if (caseObj != null)
            {
                db.Cases.Remove(caseObj);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
