using NotesMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class RejectedNotesAdminController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

        [Route("NotesRejected")]
        // GET: RejectedNotesAdmin
        public ActionResult NotesRejected(string Search, int? page, string SortOrder, string SellerName)
        {
            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(Search)||x.NoteCategories.Name.Contains(Search)||x.Users.FirstName.Contains(Search)
            || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search)
            || x.AdminRemarks.Contains(Search) || Search == null)).ToList();
            List<NoteCategories> CategoryNamePublished = objAdmin.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Removed" && x.IsActive == true).ToList();
            List<Users> UserDetails = objAdmin.Users.ToList();


            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";

            ViewBag.SellerSortParam = SortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.RejectedBySortParam = SortOrder == "RejectedBy" ? "RejectedBy_desc" : "RejectedBy";

            var NotesRejected = (from nt in NoteTitlePublished
                                    join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                    from cn in table1.ToList()
                                    join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                    from sn in table2.ToList()
                                    join us in UserDetails on nt.SellerID equals us.UserID into table3
                                    from us in table3.ToList()
                                    join ad in UserDetails on nt.ActionedBy equals ad.UserID into table4
                                    from ad in table4.ToList()
                                    where ((sn.Value == "Rejected") && ((us.FirstName + us.LastName) == SellerName || string.IsNullOrEmpty(SellerName)))
                                    select new RejectedNoteAdmin
                                    {
                                        NoteDetails = nt,
                                        Category = cn,
                                        status = sn,
                                        user = us,
                                        admin = ad
                                    }).AsQueryable();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    NotesRejected = NotesRejected.OrderBy(x => x.NoteDetails.ModifiedDate);
                    break;
                case "Title_desc":
                    NotesRejected = NotesRejected.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    NotesRejected = NotesRejected.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    NotesRejected = NotesRejected.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    NotesRejected = NotesRejected.OrderBy(x => x.Category.Name);
                    break;

                case "Seller_desc":
                    NotesRejected = NotesRejected.OrderByDescending(x => x.user.FirstName);
                    break;
                case "Seller":
                    NotesRejected = NotesRejected.OrderBy(x => x.user.FirstName);
                    break;
                case "RejectedBy_desc":
                    NotesRejected = NotesRejected.OrderByDescending(x => x.admin.FirstName);
                    break;
                case "RejectedBy":
                    NotesRejected = NotesRejected.OrderBy(x => x.admin.FirstName);
                    break;
                default:
                    NotesRejected = NotesRejected.OrderByDescending(x => x.NoteDetails.ModifiedDate);
                    break;
            }

            var Seller = objAdmin.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1 && x.IsActive == true)
    .Select(s => new
    {
        Text = s.FirstName + "" + s.LastName,
    }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            /*ViewBag.SellerName = objAdmin.Users.Select(x => new { x.FirstName }).Distinct().Where(x => x.FirstName != null).ToList();*/
            /* ViewBag.SellerName = objAdmin.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1).Select(x =>x.FirstName + x.LastName).Distinct().ToList();*/
            ViewBag.NotesRejected = NotesRejected.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }

        [Route("ApproveRejected/{id}")]
        [HttpGet]
        public ActionResult ApproveRejected(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Emailid = User.Identity.Name.ToString();
            Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            SellerNotes note = objAdmin.SellerNotes.Find(id);

            if (note == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            note.Status = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value == "Published").Select(x => x.ReferenceID).FirstOrDefault();
            note.ModifiedBy = user.UserID;
            note.ModifiedDate = DateTime.Now;
            note.ActionedBy = user.UserID;
            note.PublishedDate = DateTime.Now;
            note.AdminRemarks = null;
            objAdmin.Entry(note).State = EntityState.Modified;
            objAdmin.SaveChanges();


            TempData["RejectedApprove"] = user.FirstName + " " + user.LastName;
            TempData["title"] = note.Title;
            TempData["Message"] = ",Approved Successfully !";

            return RedirectToAction("NotesRejected", "Admin");
        }
    }
}