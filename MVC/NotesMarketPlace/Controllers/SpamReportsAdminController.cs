using NotesMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class SpamReportsAdminController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

        [Route("SpamReports")]
        // GET: SpamReportsAdmin
        public ActionResult SpamReports(string Search, int? page, string SortOrder)
        {
            var name = objAdmin.Users.Select(x=>x.FirstName).ToList();
 
            List<SellerNotesReportedIssues> reports = objAdmin.SellerNotesReportedIssues.Where(x => x.IsActive == true && (x.Users.FirstName.Contains(Search)||x.Users.LastName.Contains(Search) ||x.Remarks.Contains(Search) 
            || x.SellerNotes.Title.Contains(Search) || x.SellerNotes.NoteCategories.Name.Contains(Search)
           || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search)
            || Search == null)).ToList();
            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x => x.IsActive == true).ToList();
            List<NoteCategories> CategoryNamePublished = objAdmin.NoteCategories.ToList();
            List<Users> UserDetails = objAdmin.Users.ToList();

            var SpamReport = objAdmin.SellerNotesReportedIssues.Count();
            TempData["SpamReportsCount"] = SpamReport;
            TempData.Keep("SpamReportsCount");

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title"; 
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.ReportedBySortParam = SortOrder == "ReportedBy" ? "ReportedBy_desc" : "ReportedBy";

            /*var date = reports.Where(x => x.ModifiedDate.Value.Date == Convert.ToDateTime(Search).Date).ToList().Any();*/
            /*var date2 = Convert.ToDateTime(Search).ToString("dd-MM-yyyy");*/
           /* List<SellerNotesReportedIssues> report3 = objAdmin.SellerNotesReportedIssues.ToList();*/
            /*report3 = report3.All(x => x.ModifiedDate.Value.ToString("dd-MM-yyyy") == Search || Search == null).ToString();*/
            /*List<SellerNotesReportedIssues> report3 = objAdmin.SellerNotesReportedIssues.Where(x=>x.IsActive == true && (x.ModifiedDate.Value.ToString("dd-MM-yyyy") == Search || Search == null) ).ToList();*/

            var NotesSpamReport = (from sr in reports
                                 join nt in NoteTitlePublished on sr.NoteID equals nt.SellerNotesID into table1
                                 from nt in table1.ToList()
                                 join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table2
                                 from cn in table2.ToList()                                 
                                 join us in UserDetails on sr.ReportedBy equals us.UserID into table3
                                 from us in table3.ToList()                            
                                 select new SpamReportedAdmin
                                 {
                                     Reports = sr,
                                     NoteDetails = nt,
                                     Category = cn,
                                     user = us
                                 }).AsQueryable();
            
           
           /* if (NotesSpamReport.Where(x => x.Reports.ModifiedDate.Value.ToShortDateString() == Search).ToList().Any())
            {
                NotesSpamReport = NotesSpamReport.Where(x => x.Reports.ModifiedDate.Value.ToShortDateString() == Search || Search == null);
            }*/

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.Reports.ModifiedDate);
                    break;
                case "Title_desc":
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.NoteDetails.Title); 
                    break;
                case "Category_desc":
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.Category.Name);
                    break;
                case "ReportedBy_desc":
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.user.FirstName);
                    break;
                case "ReportedBy":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.user.FirstName);
                    break;
                default:
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.Reports.ModifiedDate);
                    break;
            }

            ViewBag.NotesSpamReport = NotesSpamReport.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }

        [Route("DeleteSpamReport/{id}")]
        public ActionResult DeleteSpamReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotesReportedIssues report = objAdmin.SellerNotesReportedIssues.Find(id);
            if (report == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            objAdmin.SellerNotesReportedIssues.Remove(report);
            objAdmin.SaveChanges();

            var Emailid = User.Identity.Name.ToString();
            Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
            TempData["SpamReport"] = user.FirstName + " " + user.LastName;
  
            TempData["Message"] = ",Deleted Successfully !";

            return RedirectToAction("SpamReports", "Admin");
        }

    }
}