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
    public class AdminMemberDetailsController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

        [Route("MembersAdmin")]
        public ActionResult MembersAdmin(int? page,string SortOrder,string Search)
        {
            var users = objAdmin.Users.Where(x => x.RoleID == objAdmin.UserRoles.Where(y => y.Name.ToLower() == "member").Select(y => y.RoleID).FirstOrDefault() && x.IsEmailVerified == true && x.IsActive == true && ((x.FirstName.Contains(Search) || x.LastName.Contains(Search) || x.EmailID.Contains(Search)
            || (x.CreatedDate.Value.Day + "-" + x.CreatedDate.Value.Month + "-" + x.CreatedDate.Value.Year).Contains(Search)
            || x.Downloads1.Where(y=>y.Downloader == x.UserID && y.IsSellerHasAllowedDownload == true && y.AttachmentPath != null).Select(y=>y.PurchasedPrice).Sum().ToString().StartsWith(Search)
            || x.Downloads.Where(y => y.Seller == x.UserID && y.IsSellerHasAllowedDownload == true && y.AttachmentPath != null).Select(y => y.PurchasedPrice).Sum().ToString().StartsWith(Search)
            || Search == null)));

            /*var notes = objAdmin.SellerNotes.ToList();
            var status = objAdmin.ReferenceData.ToList();
            var downloads = objAdmin.Downloads.ToList();*/

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.FirstNameSortParam = SortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewBag.LastNameSortParam = SortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.EmailIDSortParam = SortOrder == "EmailID" ? "EmailID_desc" : "EmailID"; 

            ViewBag.Download = objAdmin.Downloads.Where(x => x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null && x.IsAttachmentDownloaded == true ).ToList();
            ViewBag.status = objAdmin.ReferenceData.Where(x => x.IsActive == true).ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    users = users.OrderBy(x => x.CreatedDate);
                    break;
                case "FirstName_desc":
                    users = users.OrderByDescending(x => x.FirstName);
                    break;
                case "FirstName":
                    users = users.OrderBy(x => x.FirstName);
                    break;
                case "LastName_desc":
                    users = users.OrderByDescending(x => x.LastName);
                    break;
                case "LastName":
                    users = users.OrderBy(x => x.LastName);
                    break;
                case "EmailID_desc":
                    users = users.OrderByDescending(x => x.EmailID);
                    break;
                case "EmailID":
                    users = users.OrderBy(x => x.EmailID);
                    break;
                default:
                    users = users.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            /* ViewBag.AllMembers = users.ToList().ToPagedList(page ?? 1, 2);*/
            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }

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
                var name = objAdmin.Users.Where(x => x.UserID == id).Select(x=>x.FirstName +" "+x.LastName).FirstOrDefault();
                TempData["profile"] = name;
                if (TempData["profile"] != null)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Error", "HomePage");
                }
            }

           /* if (memberdetails == null)
            {
                return RedirectToAction("Error", "HomePage");
            }*/

            Users user = objAdmin.Users.Find(id);
            ViewBag.User = user;
            /*ViewBag.LastName = user.LastName;
            ViewBag.EmailId = user.EmailID;*/

            ViewBag.MemberDetails = memberdetails;

           /* var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();*/

            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x => x.SellerID == id && x.IsActive == true && (x.Title.Contains(SearchPublished)||x.NoteCategories.Name.Contains(SearchPublished)||x.ReferenceData.Value.Contains(SearchPublished)
             || (x.PublishedDate.Value.Day + "-" + x.PublishedDate.Value.Month + "-" + x.PublishedDate.Value.Year).Contains(SearchPublished)
              || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(SearchPublished)
            || SearchPublished == null)).ToList();

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
                                     status = sn
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
            ViewBag.MemberAllNotes = MemberAllNotes.ToList().ToPagedList(PublishedNotespage ?? 1, 5);
            return View();
        }


        [Route("DeactivateMember/{id}")]
        public ActionResult DeactivateMember(int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = objAdmin.Users.Find(id);

            users.IsActive = false;
            objAdmin.Entry(users).State = EntityState.Modified;
            objAdmin.SaveChanges();

            var userid = users.UserID;

            var notes = objAdmin.SellerNotes.Where(x => x.SellerID == userid);
            foreach(var item in notes)
            {
                item.Status = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value.ToLower() == "removed").Select(x => x.ReferenceID).FirstOrDefault();
                item.ActionedBy = user.UserID;
                item.ModifiedBy = user.UserID;
                item.ModifiedDate = DateTime.Now;
                item.AdminRemarks = null;
                objAdmin.Entry(item).State = EntityState.Modified;        
            }
            objAdmin.SaveChanges();

            TempData["Deactivate"] = users.FirstName+" "+users.LastName;
            TempData["Message"] = "Deactivate Successfully !";

            return RedirectToAction("MembersAdmin","Admin");

        }
    }
}