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
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class AdminMemberDetailsController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();
        [Route("MemberDetails/{id}")]
        // GET: AdminMemberDetails
        public ActionResult MemberDetails(int? id,string SortOrderPublished,int? PublishedNotespage,string SearchPublished)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserProfile memberdetails = objAdmin.UserProfile.Where(x=>x.UserID == id).FirstOrDefault();

            if (memberdetails == null)
            {
                return HttpNotFound();
            }

            Users user = objAdmin.Users.Find(id);
            ViewBag.User = user;
            /*ViewBag.LastName = user.LastName;
            ViewBag.EmailId = user.EmailID;*/

            ViewBag.MemberDetails = memberdetails;

           /* var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();*/

            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x => x.SellerID == id && x.IsActive == true && (x.Title.Contains(SearchPublished) || SearchPublished == null)).ToList();
            List<NoteCategories> CategoryNamePublished = objAdmin.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" &&  x.IsActive == true).ToList();
            List<Downloads> DonwloadsNote = objAdmin.Downloads.ToList();
            ViewBag.DateSortParamPublish = string.IsNullOrEmpty(SortOrderPublished) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParamPublish = SortOrderPublished == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParamPublish = SortOrderPublished == "Category" ? "Category_desc" : "Category";
            ViewBag.StatusSortParamPublish = SortOrderPublished == "Status" ? "Status_desc" : "Status";
            ViewBag.DownloadSortParamPublish = SortOrderPublished == "Download" ? "Download_desc" : "Download";
            ViewBag.EarningSortParamPublish = SortOrderPublished == "Earning" ? "Earning_desc" : "Earning";
            ViewBag.PublishedDateSortParamPublish = SortOrderPublished == "PublishedDate" ? "PublishedDate_desc" : "PublishedDate";

            var MemberAllNotes = (from nt in NoteTitlePublished
                                 join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                 from cn in table1.ToList()
                                 join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                 from sn in table2.ToList()
                                 /*join dn in DonwloadsNote on nt.SellerNotesID equals dn.NoteID into table3
                                 from dn in table3.ToList()*/
                                  where sn.Value != "Draft"
                                 select new MemberNotes
                                 {
                                     NoteDetails = nt,
                                     Category = cn,
                                     status = sn,
                                    /* Download = dn*/
                                 }).AsQueryable();

            switch (SortOrderPublished)
            {
                case "CreatedDate_asc":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.NoteDetails.ModifiedDate);
                    break;
                case "Title_desc":
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.Category.Name);
                    break;
                case "Status_desc":
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.status.Value);
                    break;
                case "Status":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.status.Value);
                    break;
                case "Download_desc":
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.status.Value);
                    break;
                case "Download":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.status.Value);
                    break;
                case "Earning_desc":
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.status.Value);
                    break;
                case "Earning":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.status.Value);
                    break;
                case "PublishedDate_desc":
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.NoteDetails.PublishedDate);
                    break;
                case "PublishedDate":
                    MemberAllNotes = MemberAllNotes.OrderBy(x => x.NoteDetails.PublishedDate);
                    break;
                default:
                    MemberAllNotes = MemberAllNotes.OrderByDescending(x => x.NoteDetails.ModifiedDate);
                    break;
            }
            ViewBag.MemberAllNotes = MemberAllNotes.ToList().ToPagedList(PublishedNotespage ?? 1, 4);
            return View();
        }
    }
}