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
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class NotesUnderReviewController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

        [Route("NotesUnderReview")]
        // GET: NotesUnderReview
        public ActionResult NotesUnderReview(string SearchUnderReview, int? NotesUnderReviewspage, string SortOrderUnderReview,string SellerName)
        {
                List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(SearchUnderReview) || SearchUnderReview == null)).ToList();
                List<NoteCategories> CategoryNamePublished = objAdmin.NoteCategories.ToList();
                List<ReferenceData> StatusNamePublished = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();
                List<Users> UserDetails = objAdmin.Users.ToList();
               

                ViewBag.DateSortParamPublish = string.IsNullOrEmpty(SortOrderUnderReview) ? "CreatedDate_asc" : "";
                ViewBag.TitleSortParamPublish = SortOrderUnderReview == "Title" ? "Title_desc" : "Title";
                ViewBag.CategorySortParamPublish = SortOrderUnderReview == "Category" ? "Category_desc" : "Category";
             
                ViewBag.SellerSortParamPublish = SortOrderUnderReview == "Seller" ? "Seller_desc" : "Seller";
                ViewBag.StatusSortParamPublish = SortOrderUnderReview == "Status" ? "Status_desc" : "Status";

                var NotesUnderReview = (from nt in NoteTitlePublished
                                     join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                     from cn in table1.ToList()
                                     join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                     from sn in table2.ToList()
                                     join us in UserDetails on nt.SellerID equals us.UserID into table3
                                     from us in table3.ToList()
                                     where ((sn.Value == "Submitted For Review" || sn.Value == "In Review") && ((us.FirstName + us.LastName) == SellerName || string.IsNullOrEmpty(SellerName)))
                                     select new UnderReviewsNote
                                     {
                                         NoteDetails = nt,
                                         Category = cn,
                                         status = sn,
                                         user = us,
                                         
                                     }).AsQueryable();

                switch (SortOrderUnderReview)
                {
                    case "CreatedDate_asc":
                        NotesUnderReview = NotesUnderReview.OrderBy(x => x.NoteDetails.ModifiedDate);
                        break;
                    case "Title_desc":
                        NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.NoteDetails.Title);
                        break;
                    case "Title":
                        NotesUnderReview = NotesUnderReview.OrderBy(x => x.NoteDetails.Title);
                        break;
                    case "Category_desc":
                        NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.Category.Name);
                        break;
                    case "Category":
                        NotesUnderReview = NotesUnderReview.OrderBy(x => x.Category.Name);
                        break;
                    
                    case "Seller_desc":
                        NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.user.FirstName);
                        break;
                    case "Seller":
                        NotesUnderReview = NotesUnderReview.OrderBy(x => x.user.FirstName);
                        break;
                    case "Status_desc":
                        NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.status.Value);
                        break;
                    case "Status":
                        NotesUnderReview = NotesUnderReview.OrderBy(x => x.status.Value);
                        break;
                    default:
                        NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.NoteDetails.ModifiedDate);
                        break;
                }

            var Seller = objAdmin.Users.Where(x=>x.IsEmailVerified == true && x.RoleID ==1)
    .Select(s => new
    {
        Text = s.FirstName + "" + s.LastName,
    }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");

            /*ViewBag.SellerName = objAdmin.Users.Select(x => new { x.FirstName }).Distinct().Where(x => x.FirstName != null).ToList();*/
           /* ViewBag.SellerName = objAdmin.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1).Select(x =>x.FirstName + x.LastName).Distinct().ToList();*/
            ViewBag.NotesUnderReview = NotesUnderReview.ToList().ToPagedList(NotesUnderReviewspage ?? 1, 4);
                return View();
        }
   
        [Route("Approve/{id}")]
        [HttpGet]
        public ActionResult Approve(int? id)
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
                return HttpNotFound();
            }

            note.Status = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value == "Published").Select(x => x.ReferenceID).FirstOrDefault();
            note.ModifiedBy = user.UserID;
            note.ModifiedDate = DateTime.Now;
            note.ActionedBy = user.UserID;
            note.PublishedDate = DateTime.Now;
            objAdmin.Entry(note).State = EntityState.Modified;
            objAdmin.SaveChanges();

            return RedirectToAction("NotesUnderReview","Admin");
        }

        [Route("InReview/{id}")]
        [HttpGet]
        public ActionResult InReview(int? id)
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
                return HttpNotFound();
            }

            note.Status = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value == "In Review").Select(x => x.ReferenceID).FirstOrDefault();
            note.ModifiedBy = user.UserID;
            note.ModifiedDate = DateTime.Now;
            note.ActionedBy = user.UserID;
            objAdmin.Entry(note).State = EntityState.Modified;
            objAdmin.SaveChanges();
            return RedirectToAction("NotesUnderReview", "Admin");
        }

        [Route("RejectedNote/{id}")]
        [HttpGet]
        public ActionResult RejectedNote(int? id, AdminRemarks adminremark)
        {
            var Emailid = User.Identity.Name.ToString();
            Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotes note = objAdmin.SellerNotes.Find(id);

            if (note == null)
            {
                return HttpNotFound();
            }

            note.Status = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value.ToLower() == "rejected").Select(x => x.ReferenceID).FirstOrDefault();
            note.ModifiedBy = user.UserID;
            note.ModifiedDate = DateTime.Now;
            note.AdminRemarks = adminremark.Remarks;
            note.ActionedBy = user.UserID;
            objAdmin.Entry(note).State = EntityState.Modified;
            objAdmin.SaveChanges();

            return RedirectToAction("NotesUnderReview", "Admin");
        }

    }
}