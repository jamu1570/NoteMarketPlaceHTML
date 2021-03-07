using NotesMarketPlace.EmailTemplates;
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
    public class AdminPageController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();
        // GET: AdminPage
        [Route("Dashboard")]
        public ActionResult Dashboard(string SearchPublished, int? PublishedNotespage, string SortOrderPublished)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();


            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x=> x.IsActive == true && (x.Title.Contains(SearchPublished) || SearchPublished == null)).ToList();
            List<NoteCategories> CategoryNamePublished = objAdmin.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();
            List<Users> UserDetails = objAdmin.Users.ToList();
            List<SellerNotesAttachments> attachmentDetails = objAdmin.SellerNotesAttachments.ToList();

            ViewBag.DateSortParamPublish = string.IsNullOrEmpty(SortOrderPublished) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParamPublish = SortOrderPublished == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParamPublish = SortOrderPublished == "Category" ? "Category_desc" : "Category";
            ViewBag.SellTypeSortParamPublish = SortOrderPublished == "Type" ? "Type_desc" : "Type";
            ViewBag.PriceSortParamPublish = SortOrderPublished == "Price" ? "Price_desc" : "Price";

            ViewBag.SizeSortParamPublish = SortOrderPublished == "Size" ? "Size_desc" : "Size";
            ViewBag.PublisherSortParamPublish = SortOrderPublished == "Publisher" ? "Publisher_desc" : "Publisher";
            ViewBag.DownloadsSortParamPublish = SortOrderPublished == "Downloads" ? "Downloads_desc" : "Downloads";

            var PublishedNote = (from nt in NoteTitlePublished
                                 join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                 from cn in table1.ToList()
                                 join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                 from sn in table2.ToList()
                                 join us in UserDetails on nt.SellerID equals us.UserID into table3
                                 from us in table3.ToList()
                                 join ad in attachmentDetails on nt.SellerNotesID equals ad.NoteID into table4
                                 from ad in table4.ToList()
                                 where sn.Value == "Published"
                               
                                 select new AllPublishedNote
                                 {
                                     NoteDetails = nt,
                                     Category = cn,
                                     status = sn,
                                     user = us,
                                     attachment = ad
                                 }).AsQueryable();

            switch (SortOrderPublished)
            {
                case "CreatedDate_asc":
                    PublishedNote = PublishedNote.OrderBy(x => x.NoteDetails.PublishedDate);
                    break;
                case "Title_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    PublishedNote = PublishedNote.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    PublishedNote = PublishedNote.OrderBy(x => x.Category.Name);
                    break;
                case "Type_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.NoteDetails.IsPaid);
                    break;
                case "Type":
                    PublishedNote = PublishedNote.OrderBy(x => x.NoteDetails.IsPaid);
                    break;
                case "Price_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.NoteDetails.SellingPrice);
                    break;
                case "Price":
                    PublishedNote = PublishedNote.OrderBy(x => x.NoteDetails.SellingPrice);
                    break;
                case "Size_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.attachment.AttachmentSize);
                    break;
                case "Size":
                    PublishedNote = PublishedNote.OrderBy(x => x.attachment.AttachmentSize);
                    break;
                case "Publisher_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.user.FirstName);
                    break;
                case "Publisher":
                    PublishedNote = PublishedNote.OrderBy(x => x.user.FirstName);
                    break;
                case "Downloads_desc":
                    PublishedNote = PublishedNote.OrderByDescending(x => x.NoteDetails.SellingPrice);
                    break;
                case "Downloads":
                    PublishedNote = PublishedNote.OrderBy(x => x.NoteDetails.SellingPrice);
                    break;
                default:
                    PublishedNote = PublishedNote.OrderByDescending(x => x.NoteDetails.PublishedDate);
                    break;
            }

            ViewBag.PublishedNote = PublishedNote.ToList().ToPagedList(PublishedNotespage ?? 1, 4);
            return View();  
        }

        [Route("Unpublished/{id}")]
        [HttpGet]
        public ActionResult Unpublished(int? id,UnpublishedNote unpublished)
        {
          /*  var Emailid = User.Identity.Name.ToString();
            Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();*/

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotes note = objAdmin.SellerNotes.Where(x => x.SellerNotesID == id).FirstOrDefault();

            Users user = objAdmin.Users.Find(note.SellerID);

            note.Status = objAdmin.ReferenceData.Where(x=>x.RefCategory == "Note Status" && x.Value.ToLower() == "removed").Select(x=>x.ReferenceID).FirstOrDefault();
            note.ModifiedBy = user.UserID;
            note.ModifiedDate = DateTime.Now;
            objAdmin.Entry(note).State = EntityState.Modified;
            objAdmin.SaveChanges();
            
            string Remarks = unpublished.Remarks;

            string title = note.Title;

            UnPublishednote.UnPublishedNoteNotify(user,title,Remarks);
            
            return RedirectToAction("Dashboard","Admin");
        }

        [Route("NoteDetailsAdmin/{id}")]
        public ActionResult NoteDetailsAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotes sellerNote = objAdmin.SellerNotes.Find(id);


            if (sellerNote == null)
            {
                return HttpNotFound();
            }
            Countries country = objAdmin.Countries.Find(sellerNote.Country);
            NoteCategories category = objAdmin.NoteCategories.Find(sellerNote.Category);
            SellerNotesAttachments attechment = objAdmin.SellerNotesAttachments.Where(x => x.NoteID == sellerNote.SellerNotesID).FirstOrDefault();
            if (country == null)
            {
                ViewBag.Country = null;
            }
            else
            {
                ViewBag.Country = country.Name;
            }

            ViewBag.Category = category.Name;
            ViewBag.Attchment = attechment.FilePath;

            return View(sellerNote);

        }


    }
}