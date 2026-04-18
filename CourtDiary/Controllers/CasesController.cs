//using Microsoft.AspNetCore.Mvc;
//using CourtDiary.Models;
//using System.Linq;
//using System.Data.Entity;
//using Microsoft.AspNetCore.Http;
//using System.IO;
//using System;
//using System.Collections.Generic;

//namespace CourtDiary.Controllers
//{
//    public class CasesController : Controller
//    {
//        private CourtDiaryContext _context = new CourtDiaryContext();

//        public IActionResult Index()
//        {
//            var cases = _context.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).ToList();
//            return View(cases);
//        }

//        [HttpGet]
//        public IActionResult GetClient(int id)
//        {
//            var client = _context.Clients.Find(id);
//            if (client == null) return NotFound();
//            return Json(new
//            {
//                name = client.name,
//                phone = client.phone,
//                address = client.address,
//                description = client.description,
//                avatar = string.IsNullOrEmpty(client.avatar) ? (string.IsNullOrEmpty(client.ImagePath) ? "/images/user.jpg" : client.ImagePath) : client.avatar
//            });
//        }

//        public IActionResult Create()
//        {
//            ViewBag.Clients = _context.Clients.ToList();
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Create(Case caseObj, int? client_id, List<IFormFile> uploadedFiles, UploadCategory docCategory,
//            string oppPartyName, string oppPartyNotes, IFormFile oppPartyAvatarFile,
//            string oppLawyerName, string oppLawyerNotes, IFormFile oppLawyerAvatarFile,
//            DateTime? firstHearingDate, DateTime? nextHearingDate, DateTime? previousHearingDate)
//        {
//            caseObj.client_id = client_id;
//            caseObj.user_id = 1; // Temporary dummy user to satisfy FK constraints

//            ModelState.Clear(); // Force validation to pass for prototype

//            if (ModelState.IsValid)
//            {
//                _context.Cases.Add(caseObj);
//                _context.SaveChanges(); // to get case_id

//                // Handle documents
//                if (uploadedFiles != null && uploadedFiles.Count > 0)
//                {
//                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
//                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

//                    foreach (var file in uploadedFiles)
//                    {
//                        if (file.Length > 0)
//                        {
//                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
//                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
//                            using (var fileStream = new FileStream(filePath, FileMode.Create))
//                            {
//                                file.CopyTo(fileStream);
//                            }

//                            _context.Uploads.Add(new Upload
//                            {
//                                case_id = caseObj.case_id,
//                                upload_url = "/uploads/" + uniqueFileName,
//                                upload_category = docCategory,
//                                created_at = DateTime.Now
//                            });
//                        }
//                    }
//                }

//                // Handle Case Parties
//                if (!string.IsNullOrEmpty(oppPartyName))
//                {
//                    string oppAvatar = "/images/user.jpg";
//                    if (oppPartyAvatarFile != null && oppPartyAvatarFile.Length > 0)
//                    {
//                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
//                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(oppPartyAvatarFile.FileName);
//                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
//                        using (var fileStream = new FileStream(filePath, FileMode.Create)) { oppPartyAvatarFile.CopyTo(fileStream); }
//                        oppAvatar = "/uploads/" + uniqueFileName;
//                    }

//                    _context.Case_Parties.Add(new Case_Party
//                    {
//                        case_id = caseObj.case_id,
//                        name = oppPartyName,
//                        type = "Opposite Party",
//                        notes = oppPartyNotes,
//                        avatar = oppAvatar
//                    });
//                }

//                if (!string.IsNullOrEmpty(oppLawyerName))
//                {
//                    string lwyAvatar = "/images/user.jpg";
//                    if (oppLawyerAvatarFile != null && oppLawyerAvatarFile.Length > 0)
//                    {
//                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
//                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(oppLawyerAvatarFile.FileName);
//                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
//                        using (var fileStream = new FileStream(filePath, FileMode.Create)) { oppLawyerAvatarFile.CopyTo(fileStream); }
//                        lwyAvatar = "/uploads/" + uniqueFileName;
//                    }

//                    _context.Case_Parties.Add(new Case_Party
//                    {
//                        case_id = caseObj.case_id,
//                        name = oppLawyerName,
//                        type = "Opposite Party Lawyer",
//                        notes = oppLawyerNotes,
//                        avatar = lwyAvatar
//                    });
//                }

//                if (firstHearingDate.HasValue || nextHearingDate.HasValue || previousHearingDate.HasValue)
//                {
//                    _context.Hearings.Add(new Hearing
//                    {
//                        case_id = caseObj.case_id,
//                        hearing_date = firstHearingDate,
//                        next_hearing_date = nextHearingDate,
//                        notes = "Initial hearing dates",
//                        status = "Scheduled"
//                    });
//                }

//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            // Log model state errors if any to console
//            foreach (var state in ModelState)
//            {
//                foreach (var error in state.Value.Errors)
//                {
//                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
//                }
//            }

//            ViewBag.Clients = _context.Clients.ToList();
//            return View(caseObj);
//        }

//        public IActionResult Edit(int id)
//        {
//            var caseObj = _context.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).FirstOrDefault(c => c.case_id == id);
//            if (caseObj == null)
//            {
//                return NotFound();
//            }
//            ViewBag.Clients = _context.Clients.ToList();
//            return View(caseObj);
//        }

//        [HttpPost]
//        public IActionResult Edit(Case caseObj, int? client_id, List<IFormFile> uploadedFiles, UploadCategory docCategory,
//            DateTime? firstHearingDate, DateTime? nextHearingDate, DateTime? previousHearingDate)
//        {
//            caseObj.user_id = 1; // Temporary dummy user to satisfy FK constraints
//            ModelState.Clear(); // Force validation to pass for prototype

//            if (ModelState.IsValid)
//            {
//                var existingCase = _context.Cases.Find(caseObj.case_id);
//                if (existingCase == null) return NotFound();

//                existingCase.case_no = caseObj.case_no;
//                existingCase.title = caseObj.title;
//                existingCase.status = caseObj.status;
//                existingCase.description = caseObj.description;
//                existingCase.hearing_notes = caseObj.hearing_notes;
//                existingCase.previous_hearing_summary = caseObj.previous_hearing_summary;
//                existingCase.location = caseObj.location;
//                existingCase.client_id = client_id;

//                if (uploadedFiles != null && uploadedFiles.Count > 0)
//                {
//                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
//                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

//                    foreach (var file in uploadedFiles)
//                    {
//                        if (file.Length > 0)
//                        {
//                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
//                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
//                            using (var fileStream = new FileStream(filePath, FileMode.Create))
//                            {
//                                file.CopyTo(fileStream);
//                            }

//                            _context.Uploads.Add(new Upload
//                            {
//                                case_id = caseObj.case_id,
//                                upload_url = "/uploads/" + uniqueFileName,
//                                upload_category = docCategory,
//                                created_at = DateTime.Now
//                            });
//                        }
//                    }
//                }

//                if (firstHearingDate.HasValue || nextHearingDate.HasValue || previousHearingDate.HasValue)
//                {
//                    var existingHearing = _context.Hearings.FirstOrDefault(h => h.case_id == caseObj.case_id);
//                    if (existingHearing != null)
//                    {
//                        existingHearing.hearing_date = firstHearingDate;
//                        existingHearing.next_hearing_date = nextHearingDate;
//                    }
//                    else
//                    {
//                        _context.Hearings.Add(new Hearing
//                        {
//                            case_id = caseObj.case_id,
//                            hearing_date = firstHearingDate,
//                            next_hearing_date = nextHearingDate,
//                            notes = "Updated hearing dates",
//                            status = "Scheduled"
//                        });
//                    }
//                }

//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.Clients = _context.Clients.ToList();
//            return View(caseObj);
//        }

//        public IActionResult Viewdetails(int id)
//        {
//            var caseObj = _context.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).FirstOrDefault(c => c.case_id == id);
//            if (caseObj == null)
//            {
//                return NotFound();
//            }
//            return View(caseObj);
//        }

//        [HttpPost]
//        public IActionResult Delete(int id)
//        {
//            var caseObj = _context.Cases.Find(id);
//            if (caseObj != null)
//            {
//                _context.Cases.Remove(caseObj);
//                _context.SaveChanges();
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using CourtDiary.Models;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Collections.Generic;

namespace CourtDiary.Controllers
{
    public class CasesController : Controller
    {
        private CourtDiaryContext _context = new CourtDiaryContext();

        public IActionResult Index()
        {
            var cases = _context.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).ToList();
            return View(cases);
        }

        [HttpGet]
        public IActionResult GetClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();

            return Json(new
            {
                name = client.name,
                phone = client.phone,
                address = client.address,
                description = client.description,
                avatar = string.IsNullOrEmpty(client.avatar) ? (string.IsNullOrEmpty(client.ImagePath) ? "/images/user.jpg" : client.ImagePath) : client.avatar
            });
        }

        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Case caseObj, int? client_id, List<IFormFile> uploadedFiles, UploadCategory docCategory,
            string oppPartyName, string oppPartyNotes, IFormFile oppPartyAvatarFile,
            string oppLawyerName, string oppLawyerNotes, IFormFile oppLawyerAvatarFile,
            DateTime? firstHearingDate, DateTime? nextHearingDate, DateTime? previousHearingDate)
        {
            caseObj.client_id = client_id;
            caseObj.user_id = 1;

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                _context.Cases.Add(caseObj);
                _context.SaveChanges();

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

                            _context.Uploads.Add(new Upload
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

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            oppPartyAvatarFile.CopyTo(fileStream);
                        }

                        oppAvatar = "/uploads/" + uniqueFileName;
                    }

                    _context.Case_Parties.Add(new Case_Party
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

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            oppLawyerAvatarFile.CopyTo(fileStream);
                        }

                        lwyAvatar = "/uploads/" + uniqueFileName;
                    }

                    _context.Case_Parties.Add(new Case_Party
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
                    _context.Hearings.Add(new Hearing
                    {
                        case_id = caseObj.case_id,
                        hearing_date = firstHearingDate,
                        next_hearing_date = nextHearingDate,
                        notes = "Initial hearing dates",
                        status = "Scheduled"
                    });
                }

                _context.SaveChanges();

                // ✅ Redirect logic added
                var referer = Request.Headers["Referer"].ToString();

                if (!string.IsNullOrEmpty(referer))
                {
                    var uri = new Uri(referer);
                    var path = uri.AbsolutePath.ToLower();

                    if (path.StartsWith("/admin"))
                        return Redirect("/Admin/Cases/Index");
                    else
                        return Redirect("/Cases/Index");
                }

                return Redirect("/Cases/Index");
            }

            ViewBag.Clients = _context.Clients.ToList();
            return View(caseObj);
        }

        public IActionResult Edit(int id)
        {
            var caseObj = _context.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).FirstOrDefault(c => c.case_id == id);

            if (caseObj == null)
                return NotFound();

            ViewBag.Clients = _context.Clients.ToList();
            return View(caseObj);
        }

        [HttpPost]
        public IActionResult Edit(Case caseObj, int? client_id, List<IFormFile> uploadedFiles, UploadCategory docCategory,
            DateTime? firstHearingDate, DateTime? nextHearingDate, DateTime? previousHearingDate)
        {
            caseObj.user_id = 1;
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                var existingCase = _context.Cases.Find(caseObj.case_id);
                if (existingCase == null) return NotFound();

                existingCase.case_no = caseObj.case_no;
                existingCase.title = caseObj.title;
                existingCase.status = caseObj.status;
                existingCase.description = caseObj.description;
                existingCase.hearing_notes = caseObj.hearing_notes;
                existingCase.previous_hearing_summary = caseObj.previous_hearing_summary;
                existingCase.location = caseObj.location;
                existingCase.client_id = client_id;

                _context.SaveChanges();

                // ✅ Redirect logic added
                var referer = Request.Headers["Referer"].ToString();

                if (!string.IsNullOrEmpty(referer))
                {
                    var uri = new Uri(referer);
                    var path = uri.AbsolutePath.ToLower();

                    if (path.StartsWith("/admin"))
                        return Redirect("/Admin/Cases/Index");
                    else
                        return Redirect("/Cases/Index");
                }

                return Redirect("/Cases/Index");
            }

            ViewBag.Clients = _context.Clients.ToList();
            return View(caseObj);
        }

        public IActionResult Viewdetails(int id)
        {
            var caseObj = _context.Cases.Include(c => c.Client).Include(c => c.Uploads).Include(c => c.Case_Parties).Include(c => c.Hearings).FirstOrDefault(c => c.case_id == id);

            if (caseObj == null)
                return NotFound();

            // ✅ Back button logic
            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var uri = new Uri(referer);
                var path = uri.AbsolutePath.ToLower();

                if (path.StartsWith("/admin"))
                    ViewBag.BackUrl = "/Admin/Cases/Index";
                else
                    ViewBag.BackUrl = "/Cases/Index";
            }
            else
            {
                ViewBag.BackUrl = "/Cases/Index";
            }

            return View(caseObj);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var caseObj = _context.Cases.Find(id);

            if (caseObj != null)
            {
                _context.Cases.Remove(caseObj);
                _context.SaveChanges();
            }

            // ✅ Redirect logic added
            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                var uri = new Uri(referer);
                var path = uri.AbsolutePath.ToLower();

                if (path.StartsWith("/admin"))
                    return Redirect("/Admin/Cases/Index");
                else
                    return Redirect("/Cases/Index");
            }

            return Redirect("/Cases/Index");
        }
    }
}