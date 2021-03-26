using NotesMarketPlace.EmailTemplates;
using NotesMarketPlace.Healpers;
using NotesMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminPageController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

       
        // GET: AdminPage
        [Route("Dashboard")]
        public ActionResult Dashboard(string SearchPublished, int? PublishedNotespage, string SortOrderPublished,string SelectMonth)
        {
            
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = objAdmin.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                }
            }

            DateTime dtNow = DateTime.Now;

            var dt = DateTime.Now.AddDays(-7);

            var SpamReport = objAdmin.SellerNotesReportedIssues.Count();
            TempData["SpamReportsCount"] = SpamReport;
            TempData.Keep("SpamReportsCount");

            var LastDownloads = objAdmin.Downloads.Where(x=> x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null && x.IsAttachmentDownloaded == true && x.CreatedDate > dt).Count();
            TempData["Last7DayDownloads"] = LastDownloads;

            var LastRegistration = objAdmin.Users.Where(x => x.RoleID == (objAdmin.UserRoles.Where(y=>y.Name.ToLower() == "member").Select(y=>y.RoleID).FirstOrDefault()) && x.CreatedDate > dt).Count();
            TempData["Last7DayRegistrations"] = LastRegistration;

            var AllUnderReviewNotes = objAdmin.SellerNotes.Where(x=>x.Status == objAdmin.ReferenceData.Where(z=>z.Value.ToLower() == "submitted for review").Select(y=>y.ReferenceID).FirstOrDefault() ||
            x.Status == objAdmin.ReferenceData.Where(z => z.Value.ToLower() == "in review").Select(y => y.ReferenceID).FirstOrDefault()).Count();
            TempData["AllUnderReviewNotes"] = AllUnderReviewNotes;

            /*var databasedate = objAdmin.SellerNotes.Select(x => x.PublishedDate.Value.Day + "-" + x.PublishedDate.Value.Month + "-" + x.PublishedDate.Value.Year).Contains(Convert.ToDateTime(SearchPublished).Day + "-" + Convert.ToDateTime(SearchPublished).Month + "-" + Convert.ToDateTime(SearchPublished).Year);*/
            /*var data = objAdmin.SellerNotes.Select(x => x.PublishedDate.Value.Day + "-" + x.PublishedDate.Value.Month + "-" + x.PublishedDate.Value.Year);
            var date2 = objAdmin.SellerNotes.Select(x => x.PublishedDate.Value.Day + "-" + x.PublishedDate.Value.Month + "-" + x.PublishedDate.Value.Year).Contains(Convert.ToDateTime(SearchPublished).Day.ToString() + "-" + Convert.ToDateTime(SearchPublished).Month.ToString() + "-" + Convert.ToDateTime(SearchPublished).Year.ToString());*/
           /* String date = null;
            if (SearchPublished != null) {
                if (SearchPublished != "" && SearchPublished.Contains('-'))
                {
                    date = (Convert.ToDateTime(SearchPublished).ToString("d-M-yyyy"));
                }
            }*/
            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x=> x.IsActive == true && (x.Title.Contains(SearchPublished)||x.NoteCategories.Name.Contains(SearchPublished)||x.SellingPrice.ToString().StartsWith(SearchPublished)||x.Users.FirstName.Contains(SearchPublished)||x.Users.LastName.Contains(SearchPublished)||x.SellingPrice.ToString().StartsWith(SearchPublished)
            || (x.PublishedDate.Value.Day + "-" + x.PublishedDate.Value.Month + "-" + x.PublishedDate.Value.Year).Contains(SearchPublished)
            || SearchPublished == null)).ToList();
         
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
            int CurrentMonth = 1;
            CurrentMonth = DateTime.Now.Month;
            var now = DateTimeOffset.Now;
 
            var months = Enumerable.Range(1,6).Select(i => new
            {
                A = now.AddMonths(-i+1).ToString("MM").ToString(),
                B = now.AddMonths(-i + 1).ToString("MMMM").ToString()
            }).ToList();        

            ViewBag.SelectMonth = new SelectList(months,"A", "B");

            var PublishedNote = (from nt in NoteTitlePublished
                                 join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                 from cn in table1.ToList()
                                 join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                 from sn in table2.ToList()
                                 join us in UserDetails on nt.SellerID equals us.UserID into table3
                                 from us in table3.ToList()
                                 join ad in attachmentDetails on nt.SellerNotesID equals ad.NoteID into table4
                                 from ad in table4.ToList()
                                 where sn.Value == "Published" && (nt.PublishedDate.Value.ToString("MM") == SelectMonth
                                                                                           || string.IsNullOrEmpty(SelectMonth))
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

            ViewBag.PublishedNote = PublishedNote.ToList().ToPagedList(PublishedNotespage ?? 1, 5);
            return View();  
        }


        [Route("Unpublished/{id}")]
        [HttpGet]
        public ActionResult Unpublished(int? id,UnpublishedNote unpublished)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool internet = CheckInternet.IsConnectedToInternet();
            if (internet != true)
            {
                var Emailid = User.Identity.Name.ToString();
                Users user2 = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
                TempData["internetnotconnected"] = user2.FirstName+" "+user2.LastName;
                return RedirectToAction("Dashboard", "Member");
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
            var emailid = objAdmin.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
            var password = objAdmin.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
            UnPublishednote.UnPublishedNoteNotify(user,title,Remarks,emailid,password);

            TempData["AdminDashboard"] = user.FirstName +" "+user.LastName;
            TempData["Message"] = "Your Email has been Successfully Sent For UnPublished Note.";
            
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
                return RedirectToAction("Error","HomePage");
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

        [Route("DeleteReview/{id}")]
        public ActionResult DeleteReview(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotesReviews notereview = objAdmin.SellerNotesReviews.Find(id);

            if (notereview == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            objAdmin.SellerNotesReviews.Remove(notereview);
            objAdmin.SaveChanges();

            return RedirectToAction("Dashboard","Admin");
        }
        
        [Route("UpdateProfileAdmin")]
        [HttpGet]
        public ActionResult UpdateProfileAdmin()
        {
            ViewBag.Title = "Update Profile";
            var Emailid = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = objAdmin.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                }
            }
            var v = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (v != null)
            {
                UpdateProfileAdmin profile = new UpdateProfileAdmin
                {
                    FirstName = v.FirstName,
                    LastName = v.LastName,
                    EmailID = v.EmailID,
                    UserID = v.UserID
                };

                UserProfile ud = objAdmin.UserProfile.Where(x => x.UserID == v.UserID).FirstOrDefault();

                if (ud != null)
                {
                    
                    profile.PhoneNumberCountryCode = ud.PhoneNumberCountryCode;
                    profile.PhoneNumber = ud.PhoneNumber;
                    profile.SecondaryEmailAddress = ud.SecondaryEmailAddress;

                    ViewBag.ProfilePicture = ud.ProfilePicture;
                    /*ViewBag.ProfilePicture = Path.Combine("/SystemConfiguration/DefaultImage/", "DefaultUserImage.jpg");*/
                    ViewBag.PhoneNumberCountryCode = new SelectList(objAdmin.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode", profile.PhoneNumberCountryCode);
                    return View(profile);
                }
                else
                {
                    /*ViewBag.ProfilePicture = Path.Combine("/SystemConfiguration/DefaultImage/", "DefaultUserImage.jpg");*/
                    ViewBag.PhoneNumberCountryCode = new SelectList(objAdmin.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode");
                    return View(profile);
                }
            }
            return View();

        }
       
        [Route("UpdateProfileAdmin")]
        [HttpPost]
        public ActionResult UpdateProfileAdmin(UpdateProfileAdmin userdetails)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();

                Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                user.FirstName = userdetails.FirstName;
                user.LastName = userdetails.LastName;

                var id = objAdmin.UserProfile.Where(x => x.UserID == userdetails.UserID).FirstOrDefault();

                /* var ud = objAdmin.UserProfile.Where(x => x.UserID == user.UserID).FirstOrDefault();*/
                if (id != null)
                {
                    UserProfile updatedetail = objAdmin.UserProfile.Where(x => x.UserID == userdetails.UserID).FirstOrDefault();
                    updatedetail.PhoneNumberCountryCode = userdetails.PhoneNumberCountryCode;
                    updatedetail.PhoneNumber = userdetails.PhoneNumber;
                    updatedetail.SecondaryEmailAddress = userdetails.SecondaryEmailAddress;
                    updatedetail.ModifiedBy = userdetails.UserID;
                    updatedetail.ModifiedDate = DateTime.Now;
                    updatedetail.IsActive = true;

                    string storepath = Path.Combine(Server.MapPath("~/Members/"), user.UserID.ToString());
                    System.IO.DirectoryInfo di = new DirectoryInfo(storepath);

                    /* FileInfo file = new FileInfo(updatedetail.ProfilePicture);
                     if (file.Exists)//check file exsit or not  
                     {
                         file.Delete();

                     }*/
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    /*foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }*/

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    if (userdetails.ProfilePicture != null && userdetails.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userdetails.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userdetails.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        userdetails.ProfilePicture.SaveAs(finalpath);

                        updatedetail.ProfilePicture = Path.Combine(("/Members/" + user.UserID.ToString() + "/"), fileName);
                        objAdmin.SaveChanges();
                    }
                    else
                    {
                        updatedetail.ProfilePicture = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                        objAdmin.SaveChanges();
                    }
                    objAdmin.Entry(user).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    objAdmin.Entry(updatedetail).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    TempData["AdminDashboard"] = user.FirstName + " " + user.LastName;
                    TempData["Message"] = "! Your Profile has been Successfully Edited !";
                }
                else
                {
                    UserProfile updatedetail = new UserProfile
                    {
                        PhoneNumberCountryCode = userdetails.PhoneNumberCountryCode,
                        PhoneNumber = userdetails.PhoneNumber,
                        SecondaryEmailAddress = userdetails.SecondaryEmailAddress,                    
                    };
                    updatedetail.UserID = userdetails.UserID;
                    updatedetail.CreatedDate = DateTime.Now;
                    updatedetail.CreatedBy = userdetails.UserID;

                    string storepath = Path.Combine(Server.MapPath("~/Members/"), user.UserID.ToString());

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    if (userdetails.ProfilePicture != null && userdetails.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userdetails.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userdetails.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        userdetails.ProfilePicture.SaveAs(finalpath);

                        updatedetail.ProfilePicture = Path.Combine(("/Members/" + user.UserID.ToString() + "/"), fileName);
                        objAdmin.SaveChanges();
                    }
                    else
                    {
                        updatedetail.ProfilePicture = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                        objAdmin.SaveChanges();
                    }

                    objAdmin.UserProfile.Add(updatedetail);
                    objAdmin.SaveChanges();

                    TempData["AdminDashboard"] = user.FirstName + " " + user.LastName;
                    TempData["Message"] = "! Your Profile has been Successfully Added !";
                }

                var Records = objAdmin.SystemConfigurations.Count();
                if (Records > 0)
                {

                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("SystemConfiguration", "Admin");
                }
            }
            return View();
        }
        [Route("DownloadProfile/{id}")]
        public ActionResult DownloadProfile(int? id)
        {
            UserProfile Profilepic = objAdmin.UserProfile.Where(x => x.UserID == id).FirstOrDefault();

            var displaypath = Profilepic.ProfilePicture;
            /*SellerNotes note = objMember.SellerNotes.Find(id);*/
            /* var allFilesPath = attechment.FilePath.Split(';');*/
            string FullPath = Path.Combine(Server.MapPath("~" + displaypath));
            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "image/*", FileName);

        }
        [Route("DownloadAttechedFile/{id}")]
        public ActionResult DownloadAttechedFile(int? id)
        {
            SellerNotesAttachments attechment = objAdmin.SellerNotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();

            var allFilesPath = attechment.FilePath.Split(';');
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var FilePath in allFilesPath)
                    {
                        string FullPath = Path.Combine(Server.MapPath("~" + FilePath));
                        string FileName = Path.GetFileName(FullPath);
                        if (FileName == "")
                        {
                            continue;
                        }
                        else
                        {
                            ziparchive.CreateEntryFromFile(FullPath, FileName);
                        }
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
        }

        [Route("ChangePasswordAdmin")]
        [HttpGet]
        public ActionResult ChangePasswordAdmin()
        {
            ViewBag.Title = "NotesMarketPlace";
            return View();
        }

        [Route("ChangePasswordAdmin")]
        [HttpPost]
        public ActionResult ChangePasswordAdmin(ChangePassword changePwd)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var Emailid = User.Identity.Name.ToString();

                    Users user = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                    var pwd = EncryptPassword.EncryptPasswordMd5(changePwd.OldPassword);

                    if (user.Password == pwd)
                    {
                        user.Password = EncryptPassword.EncryptPasswordMd5(changePwd.NewPassword);
                        user.ModifiedDate = DateTime.Now;
                        user.ModifiedBy = user.UserID;

                        objAdmin.Entry(user).State = EntityState.Modified;
                        objAdmin.SaveChanges();

                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        TempData["IncorrectPwd"] = "Old Password Is Incorrect !";
                        return View();
                    }


                }
                else
                {
                    return View();
                }


            }
            return View();
        }

    }
}