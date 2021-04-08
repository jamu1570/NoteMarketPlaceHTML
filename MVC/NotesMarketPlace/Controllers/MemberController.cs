using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NotesMarketPlace.Healpers;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using System.IO.Compression;
using NotesMarketPlace.EmailTemplates;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Member")]
    [RoutePrefix("Member")]
    public class MemberController : Controller
    {
        private NotesMarketPlaceEntities objMember = new NotesMarketPlaceEntities();

        [Route("Dashboard")]
        public ActionResult Dashboard(string SearchText, string SearchPublished, int? PublishedNotespage, int? ProgressNotespage, string SortOrderProgress, string SortOrderPublished)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = objMember.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = objMember.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                }
            }

            ViewBag.DateSortParm = string.IsNullOrEmpty(SortOrderProgress) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParm = SortOrderProgress == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = SortOrderProgress == "Category" ? "Category_desc" : "Category";
            ViewBag.StatusSortParm = SortOrderProgress == "Status" ? "Status_desc" : "Status";

            List<Downloads> BuyerRecord = objMember.Downloads.Where(x => x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true).ToList();
            TempData["TotalBuyerRecords"] = BuyerRecord.Count();
            List<Downloads> MyDownloadRecord = objMember.Downloads.Where(x => x.Downloader == userObj.UserID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).ToList();
            TempData["TotalMyDownlodRecords"] = MyDownloadRecord.Count();
            List<Downloads> MySoldRecord = objMember.Downloads.Where(x => x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).ToList();
            TempData["TotalMySoldRecords"] = MySoldRecord.Count();
            TempData["TotalEarning"] = MySoldRecord.Sum(x => x.PurchasedPrice);
            List<SellerNotes> MyRejectedNote = objMember.SellerNotes.Where(x => x.SellerID == userObj.UserID && x.Status == objMember.ReferenceData.Where(y=>y.Value == "Rejected").Select(y=>y.ReferenceID).FirstOrDefault()).ToList();
            TempData["TotalMyRejectRecords"] = MyRejectedNote.Count();

            /*UserProfile Profilepic = objMember.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
            TempData["ProfilePicture"] = Profilepic.ProfilePicture;*/

            List<SellerNotes> NoteTitle = objMember.SellerNotes.Where(x => x.CreatedBy == userObj.UserID && x.IsActive == true && (x.Title.Contains(SearchText)|| x.NoteCategories.Name.Contains(SearchText)||x.ReferenceData.Value.StartsWith(SearchText) || SearchText == null)).ToList();
            List<NoteCategories> CategoryName = objMember.NoteCategories.ToList();
            List<ReferenceData> StatusName = objMember.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();

            var ProgressNote = (from nt in NoteTitle
                                join cn in CategoryName on nt.Category equals cn.CategoryID into table1
                                from cn in table1.ToList()
                                join sn in StatusName on nt.Status equals sn.ReferenceID into table2
                                from sn in table2.ToList()
                                where sn.Value != "Published" 
                                select new InProgressNote
                                {
                                    NoteDetails = nt,
                                    Category = cn,
                                    status = sn
                                }).AsQueryable();

            switch (SortOrderProgress)
            {
                case "CreatedDate_asc":
                    ProgressNote = ProgressNote.OrderBy(x => x.NoteDetails.CreatedDate);
                    break;
                case "Title_desc":
                    ProgressNote = ProgressNote.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    ProgressNote = ProgressNote.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    ProgressNote = ProgressNote.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    ProgressNote = ProgressNote.OrderBy(x => x.Category.Name);
                    break;
                case "Status_desc":
                    ProgressNote = ProgressNote.OrderByDescending(x => x.status.Value);
                    break;
                case "Status":
                    ProgressNote = ProgressNote.OrderBy(x => x.status.Value);
                    break;
                default:
                    ProgressNote = ProgressNote.OrderByDescending(x => x.NoteDetails.CreatedDate);
                    break;
            }

            ViewBag.ProgressNote = ProgressNote.ToList().ToPagedList(ProgressNotespage ?? 1, 5);

            List<SellerNotes> NoteTitlePublished = objMember.SellerNotes.Where(x => x.CreatedBy == userObj.UserID && x.IsActive == true && (x.Title.Contains(SearchPublished)||x.NoteCategories.Name.Contains(SearchPublished)||x.SellingPrice.ToString().StartsWith(SearchPublished) || SearchPublished == null)).ToList();
            List<NoteCategories> CategoryNamePublished = objMember.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = objMember.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();

            ViewBag.DateSortParamPublish = string.IsNullOrEmpty(SortOrderPublished) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParamPublish = SortOrderPublished == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParamPublish = SortOrderPublished == "Category" ? "Category_desc" : "Category";
            ViewBag.SellTypeSortParamPublish = SortOrderPublished == "Type" ? "Type_desc" : "Type";
            ViewBag.PriceSortParamPublish = SortOrderPublished == "Price" ? "Price_desc" : "Price";

            var PublishedNote = (from nt in NoteTitlePublished
                                 join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                 from cn in table1.ToList()
                                 join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                 from sn in table2.ToList()
                                 where sn.Value == "Published"
                                 select new PublishedNote
                                 {
                                     NoteDetails = nt,
                                     Category = cn,
                                     status = sn
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
                default:
                    PublishedNote = PublishedNote.OrderByDescending(x => x.NoteDetails.PublishedDate);
                    break;
            }

            ViewBag.PublishedNote = PublishedNote.ToList().ToPagedList(PublishedNotespage ?? 1, 5);
            return View();
        }

        [Route("UpdateProfile")]
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            ViewBag.Title = "Update Profile";
            var Emailid = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
            var BuyerRequest = objMember.Downloads.Where(x => x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true).Count();
            TempData["BuyerRequestCount"] = BuyerRequest;

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = objMember.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = objMember.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                }
            }
            var v = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (v != null)
            {
                UpdateProfile profile = new UpdateProfile
                {
                    FirstName = v.FirstName,
                    LastName = v.LastName,
                    EmailID = v.EmailID,
                    UserID = v.UserID
                };

                UserProfile ud = objMember.UserProfile.Where(x => x.UserID == v.UserID).FirstOrDefault();

                if (ud != null)
                {
                    profile.DOB = ud.DOB;
                    profile.Gender = ud.Gender;
                    profile.PhoneNumberCountryCode = ud.PhoneNumberCountryCode;
                    profile.PhoneNumber = ud.PhoneNumber;
                    profile.AddressLine1 = ud.AddressLine1;
                    profile.AddressLine2 = ud.AddressLine2;
                    profile.City = ud.City;
                    profile.State = ud.State;
                    profile.ZipCode = ud.ZipCode;
                    profile.Country = ud.Country;
                    profile.University = ud.University;
                    profile.College = ud.College;
                    /*profile.ProfilePicture = ud.ProfilePicture;*/
                    ViewBag.ProfilePicture = ud.ProfilePicture;
                    /*ViewBag.ProfilePicture = Path.Combine("/SystemConfiguration/DefaultImage/", "DefaultUserImage.jpg");*/
                    ViewBag.Country = new SelectList(objMember.Countries.Distinct().Where(x => x.IsActive == true), "Name", "Name", profile.Country);
                    ViewBag.PhoneNumberCountryCode = new SelectList(objMember.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode", profile.PhoneNumberCountryCode);
                    ViewBag.Gender = new SelectList(objMember.ReferenceData.Where(x => x.RefCategory == "Gender" && x.IsActive == true), "ReferenceID", "Value", profile.Gender).ToList();
                    return View(profile);
                }
                else
                {
                    /*ViewBag.ProfilePicture = Path.Combine("/SystemConfiguration/DefaultImage/", "DefaultUserImage.jpg");*/
                    ViewBag.Country = new SelectList(objMember.Countries.Distinct().Where(x => x.IsActive == true), "Name", "Name");
                    ViewBag.PhoneNumberCountryCode = new SelectList(objMember.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode");
                    ViewBag.Gender = new SelectList(objMember.ReferenceData.Where(x => x.RefCategory == "Gender" && x.IsActive == true), "ReferenceID", "Value").ToList();
                    return View(profile);
                }
            }
            return View();

        }

        [Route("UpdateProfile")]
        [HttpPost]
        public ActionResult UpdateProfile(UpdateProfile userdetails)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();

                Users user = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                user.FirstName = userdetails.FirstName;
                user.LastName = userdetails.LastName;


                var id = objMember.UserProfile.Where(x => x.UserID == userdetails.UserID).FirstOrDefault();

                /* var ud = objMember.UserProfile.Where(x => x.UserID == user.UserID).FirstOrDefault();*/
                if (id != null)
                {
                    UserProfile updatedetail = objMember.UserProfile.Where(x => x.UserID == userdetails.UserID).FirstOrDefault();
                    /*Convert.ToDateTime(userdetails.DOB).ToString("yyyy-MM-dd")*/
                    updatedetail.DOB = userdetails.DOB;
                    updatedetail.Gender = userdetails.Gender;
                    updatedetail.PhoneNumberCountryCode = userdetails.PhoneNumberCountryCode;
                    updatedetail.PhoneNumber = userdetails.PhoneNumber;
                    updatedetail.AddressLine1 = userdetails.AddressLine1;
                    updatedetail.AddressLine2 = userdetails.AddressLine2;
                    updatedetail.City = userdetails.City;
                    updatedetail.State = userdetails.State;
                    updatedetail.ZipCode = userdetails.ZipCode;
                    updatedetail.Country = userdetails.Country;
                    updatedetail.University = userdetails.University;
                    updatedetail.College = userdetails.College;
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
                        objMember.SaveChanges();
                    }
                    else
                    {

                        updatedetail.ProfilePicture = objMember.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                        objMember.SaveChanges();

                    }
                    objMember.Entry(user).State = EntityState.Modified;
                    objMember.SaveChanges();

                    objMember.Entry(updatedetail).State = EntityState.Modified;
                    objMember.SaveChanges();

                    TempData["Dashboard"] = user.FirstName + " " + user.LastName;
                    TempData["Message"] = "! Your Profile has been Successfully Updated !";
                }
                else
                {
                    UserProfile updatedetail = new UserProfile
                    {

                        DOB = userdetails.DOB,
                        Gender = userdetails.Gender,
                        PhoneNumberCountryCode = userdetails.PhoneNumberCountryCode,
                        PhoneNumber = userdetails.PhoneNumber,
                        AddressLine1 = userdetails.AddressLine1,
                        AddressLine2 = userdetails.AddressLine2,
                        City = userdetails.City,
                        State = userdetails.State,
                        ZipCode = userdetails.ZipCode,
                        Country = userdetails.Country,
                        University = userdetails.University,
                        College = userdetails.College,
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
                        objMember.SaveChanges();
                    }
                    else
                    {

                        updatedetail.ProfilePicture = objMember.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                        objMember.SaveChanges();

                    }

                    objMember.UserProfile.Add(updatedetail);
                    objMember.SaveChanges();

                    TempData["Dashboard"] = user.FirstName + " " + user.LastName;
                    TempData["Message"] = "! Your Profile has been Successfully Added !";
                }
                return RedirectToAction("Dashboard", "Member");
            }
            return View();
        }

        [Route("AddNote")]
        [HttpGet]
        public ActionResult AddNote()
        {
            ViewBag.NotesCategory = objMember.NoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesType = objMember.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = objMember.Countries.Where(x => x.IsActive == true);
            return View();

        }

        [Route("AddNote")]
        [HttpPost]
        public ActionResult AddNote(AddNote objAddNote, string submit)
        {
            if (ModelState.IsValid)
            {

                //Check UploadNote Is Selected Or Not
                if (objAddNote.UploadNotes[0] == null)
                {
                    TempData["notice"] = "Select File to upload";
                    ViewBag.NotesCategory = objMember.NoteCategories.Where(x => x.IsActive == true);
                    ViewBag.NotesType = objMember.NoteTypes.Where(x => x.IsActive == true);
                    ViewBag.Country = objMember.Countries.Where(x => x.IsActive == true);
                    return View();
                }

                if (objAddNote.IsPaid == true && objAddNote.SellingPrice == null)
                {
                    TempData["noticePrice"] = "Enter The Price";
                    ViewBag.NotesCategory = objMember.NoteCategories.Where(x => x.IsActive == true);
                    ViewBag.NotesType = objMember.NoteTypes.Where(x => x.IsActive == true);
                    ViewBag.Country = objMember.Countries.Where(x => x.IsActive == true);
                    return View();
                }

                if (objAddNote.IsPaid == true && objAddNote.NotePreview == null)
                {
                    TempData["noticePreview"] = "Note Preview Is Required For Paid Notes";
                    ViewBag.NotesCategory = objMember.NoteCategories.Where(x => x.IsActive == true);
                    ViewBag.NotesType = objMember.NoteTypes.Where(x => x.IsActive == true);
                    ViewBag.Country = objMember.Countries.Where(x => x.IsActive == true);
                    return View();
                }

                /*var EmailID = Request.Cookies["EmailID"].Value.ToString();*/
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                string path = Path.Combine(Server.MapPath("~/Members"), userObj.UserID.ToString());

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var notevalue = "";
                if (submit == "Publish") {
                    notevalue = "Submitted For Review";
                }
                else
                {
                    notevalue = "Draft";
                }

                ReferenceData referenceData = objMember.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.DataValue == notevalue && x.IsActive == true).FirstOrDefault();
                SellerNotes objSellerNote = new SellerNotes()
                {
                    SellerID = userObj.UserID,
                    Status = referenceData.ReferenceID,
                    Title = objAddNote.Title,
                    Category = objAddNote.Category,
                    Description = objAddNote.Description,
                    IsPaid = objAddNote.IsPaid,
                    NoteType = objAddNote.NoteType,
                    NumberOfPages = objAddNote.NumberOfPages,
                    UniversityName = objAddNote.UniversityName,
                    Country = objAddNote.Country,
                    Course = objAddNote.Course,
                    CourseCode = objAddNote.CourseCode,
                    Professor = objAddNote.Professor,
                    SellingPrice = objAddNote.SellingPrice,
                    CreatedBy = userObj.UserID,
                    ModifiedBy = userObj.UserID,
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                objMember.SellerNotes.Add(objSellerNote);
                objMember.SaveChanges();

                var noteID = objSellerNote.SellerNotesID;

                /* SellerNotes sellerNote = objMember.SellerNotes.Where(x => x.Title == objAddNote.Title && x.CreatedBy == userObj.UserID).FirstOrDefault();*/

                string storepath = Path.Combine(Server.MapPath("~/Members/" + userObj.UserID), noteID.ToString());

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }

                if (objAddNote.DisplayPicture != null && objAddNote.DisplayPicture.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objAddNote.DisplayPicture.FileName);
                    string extension = Path.GetExtension(objAddNote.DisplayPicture.FileName);
                    fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(storepath, fileName);
                    objAddNote.DisplayPicture.SaveAs(finalpath);

                    objSellerNote.DisplayPicture = Path.Combine(("/Members/" + userObj.UserID + "/" + noteID + "/"), fileName);
                    objMember.SaveChanges();
                }
                else
                {
                    objSellerNote.DisplayPicture = objMember.SystemConfigurations.Where(x => x.keys == "DefaultNotePicture").Select(x => x.Value).FirstOrDefault();
                    objMember.SaveChanges();
                }

                if (objAddNote.NotePreview != null && objAddNote.NotePreview.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objAddNote.NotePreview.FileName);
                    string extension = Path.GetExtension(objAddNote.NotePreview.FileName);
                    fileName = "Preview_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(storepath, fileName);
                    objAddNote.NotePreview.SaveAs(finalpath);

                    objSellerNote.NotePreview = Path.Combine(("/Members/" + userObj.UserID + "/" + noteID + "/"), fileName);
                    objMember.SaveChanges();
                }

                string attachementsstorepath = Path.Combine(storepath, "Attachements");

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(attachementsstorepath))
                {
                    Directory.CreateDirectory(attachementsstorepath);
                }

                SellerNotesAttachments sellerNotesAttachement = new SellerNotesAttachments();
                sellerNotesAttachement.NoteID = noteID;
                sellerNotesAttachement.IsActive = true;
                sellerNotesAttachement.CreatedBy = userObj.UserID;
                sellerNotesAttachement.CreatedDate = DateTime.Now;

                int Count = 1;
                var FilePath = "";
                var FileName = "";
                long FileSize = 0;
                foreach (var file in objAddNote.UploadNotes)
                {
                    FileSize += ((file.ContentLength)/1024);
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    fileName = "Attachement" + Count + "_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(attachementsstorepath, fileName);
                    file.SaveAs(finalpath);
                    FileName += fileName + ";";
                    FilePath += Path.Combine(("/Members/" + userObj.UserID + "/" + noteID + "/Attachements/"), fileName) + ";";
                    Count++;
                }

               /* decimal size = Math.Round(((decimal)FileUpload1.PostedFile.ContentLength / (decimal)1024), 2);*/
                sellerNotesAttachement.AttachmentSize =FileSize;
                sellerNotesAttachement.FileName = FileName;
                sellerNotesAttachement.FilePath = FilePath;
                objMember.SellerNotesAttachments.Add(sellerNotesAttachement);
                objMember.SaveChanges();

                TempData["Dashboard"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "! Your Note has been Successfully Added !";
                return RedirectToAction("Dashboard", "Member");
            }

            ViewBag.NotesCategory = objMember.NoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesType = objMember.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = objMember.Countries.Where(x => x.IsActive == true);
            return View();
        }

        [Route("EditNote/{id}")]
        [HttpGet]
        public ActionResult EditNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotes sellerNote = objMember.SellerNotes.Find(id);
            SellerNotesAttachments attachment = objMember.SellerNotesAttachments.Where(x => x.NoteID == sellerNote.SellerNotesID).FirstOrDefault();

            AddNote sellerNote1 = new AddNote
            {
                SellerNotesID = sellerNote.SellerNotesID,
                Title = sellerNote.Title,
                Category = sellerNote.Category,
                Description = sellerNote.Description,
                IsPaid = sellerNote.IsPaid,
                NoteType = sellerNote.NoteType,
                NumberOfPages = sellerNote.NumberOfPages,
                UniversityName = sellerNote.UniversityName,
                Country = sellerNote.Country,
                Course = sellerNote.Course,
                CourseCode = sellerNote.CourseCode,
                Professor = sellerNote.Professor,
                SellingPrice = sellerNote.SellingPrice,
            };

            if (sellerNote == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            ViewBag.NotePriview = sellerNote.NotePreview;
            ViewBag.AttechmentPath = attachment.FilePath;
            ViewBag.DP = sellerNote.DisplayPicture;
            ViewBag.Country = new SelectList(objMember.Countries, "CoutryID", "Name", sellerNote.Country);
            ViewBag.Category = new SelectList(objMember.NoteCategories, "CategoryID", "Name", sellerNote.Category);
            ViewBag.NoteType = new SelectList(objMember.NoteTypes, "TypeID", "Name", sellerNote.NoteType);

            return View(sellerNote1);

        }

        [Route("EditNote/{id}")]
        [HttpPost]
        public ActionResult EditNote(int? id, AddNote objAddNote, string submit)
        {
            if (ModelState.IsValid)
            {
                if (submit == "Publish")
                {
                    bool internet = CheckInternet.IsConnectedToInternet();
                    if (internet != true)
                    {
                        TempData["internetnotconnected"] = "User";
                        return RedirectToAction("Dashboard", "Member");
                    }
                }
                    //Check UploadNote Is Selected Or Not
                    if (objAddNote.UploadNotes[0] == null)
                {
                    TempData["notice"] = "Select File to upload";
                    ViewBag.Country = new SelectList(objMember.Countries.Where(x => x.IsActive == true), "CoutryID", "Name", objAddNote.Country);
                    ViewBag.Category = new SelectList(objMember.NoteCategories.Where(x => x.IsActive == true), "CategoryID", "Name", objAddNote.Category);
                    ViewBag.NoteType = new SelectList(objMember.NoteTypes.Where(x => x.IsActive == true), "TypeID ", "Name", objAddNote.NoteType);
                    return View(objAddNote);
                }

                if (objAddNote.IsPaid == true && objAddNote.SellingPrice == null)
                {
                    TempData["noticePrice"] = "Enter The Price";
                    ViewBag.Country = new SelectList(objMember.Countries.Where(x => x.IsActive == true), "CoutryID", "Name", objAddNote.Country);
                    ViewBag.Category = new SelectList(objMember.NoteCategories.Where(x => x.IsActive == true), "CategoryID", "Name", objAddNote.Category);
                    ViewBag.NoteType = new SelectList(objMember.NoteTypes.Where(x => x.IsActive == true), "TypeID ", "Name", objAddNote.NoteType);
                    /* ViewBag.NotesCategory = objMember.NoteCategories.Where(x => x.IsActive == true);
                     ViewBag.NotesType = objMember.NoteTypes.Where(x => x.IsActive == true);
                     ViewBag.Country = objMember.Countries.Where(x => x.IsActive == true);*/
                    return View(objAddNote);
                }

                if (objAddNote.IsPaid == true && objAddNote.NotePreview == null)
                {
                    TempData["noticePreview"] = "Note Preview Is Required For Paid Notes";
                    ViewBag.Country = new SelectList(objMember.Countries.Where(x => x.IsActive == true), "CoutryID", "Name", objAddNote.Country);
                    ViewBag.Category = new SelectList(objMember.NoteCategories.Where(x => x.IsActive == true), "CategoryID", "Name", objAddNote.Category);
                    ViewBag.NoteType = new SelectList(objMember.NoteTypes.Where(x => x.IsActive == true), "TypeID ", "Name", objAddNote.NoteType);
                    return View(objAddNote);
                }

                /*var EmailID = Request.Cookies["EmailID"].Value.ToString();*/
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                string path = Path.Combine(Server.MapPath("~/Members"), userObj.UserID.ToString());

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var notevalue = "";
                if (submit == "Publish")
                {
                    notevalue = "Submitted For Review";
                    var emailid = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
                    var password = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
                    var emails = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "emailids").Select(x => x.Value).FirstOrDefault();
                    PublishedNoteEmailcs.PublishedNoteNotify(userObj, objAddNote.Title,emailid,password,emails);
                }
                else if (submit == "Save")
                {
                    notevalue = "Draft";
                }

                ReferenceData referenceData = objMember.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.DataValue == notevalue && x.IsActive == true).FirstOrDefault();

                SellerNotes objSellerNote = objMember.SellerNotes.Where(x => x.SellerNotesID == objAddNote.SellerNotesID).FirstOrDefault();

                objSellerNote.Status = referenceData.ReferenceID;
                objSellerNote.Title = objAddNote.Title;
                objSellerNote.Category = objAddNote.Category;
                objSellerNote.Description = objAddNote.Description;
                objSellerNote.IsPaid = objAddNote.IsPaid;
                objSellerNote.NoteType = objAddNote.NoteType;
                objSellerNote.NumberOfPages = objAddNote.NumberOfPages;
                objSellerNote.UniversityName = objAddNote.UniversityName;
                objSellerNote.Country = objAddNote.Country;
                objSellerNote.Course = objAddNote.Course;
                objSellerNote.CourseCode = objAddNote.CourseCode;
                objSellerNote.Professor = objAddNote.Professor;
                objSellerNote.SellingPrice = objAddNote.SellingPrice;
                objSellerNote.CreatedBy = userObj.UserID;
                objSellerNote.IsActive = true;
                objSellerNote.ModifiedDate = DateTime.Now;
                objSellerNote.ModifiedBy = userObj.UserID;
                objMember.Entry(objSellerNote).State = EntityState.Modified;
                objMember.SaveChanges();

                var noteID = objSellerNote.SellerNotesID;
                SellerNotes sellerNote = objMember.SellerNotes.Where(x => x.Title == objAddNote.Title && x.CreatedBy == userObj.UserID).FirstOrDefault();

                string storepath = Path.Combine(Server.MapPath("~/Members/" + userObj.UserID), noteID.ToString());

                System.IO.DirectoryInfo di = new DirectoryInfo(storepath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }


                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }

                if (objAddNote.DisplayPicture != null && objAddNote.DisplayPicture.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objAddNote.DisplayPicture.FileName);
                    string extension = Path.GetExtension(objAddNote.DisplayPicture.FileName);
                    fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(storepath, fileName);
                    objAddNote.DisplayPicture.SaveAs(finalpath);

                    objSellerNote.DisplayPicture = Path.Combine(("/Members/" + userObj.UserID + "/" + noteID + "/"), fileName);
                    objMember.SaveChanges();
                }
                else
                {
                    objSellerNote.DisplayPicture = objMember.SystemConfigurations.Where(x => x.keys == "DefaultNotePicture").Select(x => x.Value).FirstOrDefault();

                    objMember.SaveChanges();

                }


                if (objAddNote.NotePreview != null && objAddNote.NotePreview.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objAddNote.NotePreview.FileName);
                    string extension = Path.GetExtension(objAddNote.NotePreview.FileName);
                    fileName = "Preview_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(storepath, fileName);
                    objAddNote.NotePreview.SaveAs(finalpath);

                    objSellerNote.NotePreview = Path.Combine(("/Members/" + userObj.UserID + "/" + noteID + "/"), fileName);
                    objMember.SaveChanges();
                }

                string attachementsstorepath = Path.Combine(storepath, "Attachements");

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(attachementsstorepath))
                {
                    Directory.CreateDirectory(attachementsstorepath);
                }

                SellerNotesAttachments sellerNotesAttachement = objMember.SellerNotesAttachments.Where(x => x.NoteID == noteID).FirstOrDefault();
                sellerNotesAttachement.ModifiedDate = DateTime.Now;
                sellerNotesAttachement.ModifiedBy = userObj.UserID;

                int Count = 1;
                var FilePath = "";
                var FileName = "";
                long FileSize = 0;
                foreach (var file in objAddNote.UploadNotes)
                {
                    FileSize += ((file.ContentLength) / 1024);
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    fileName = "Attachement" + Count + "_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(attachementsstorepath, fileName);
                    file.SaveAs(finalpath);

                    FileName += fileName + ";";
                    FilePath += Path.Combine(("/Members/" + userObj.UserID + "/" + noteID + "/Attachements/"), fileName) + ";";

                    Count++;
                }
                sellerNotesAttachement.AttachmentSize = FileSize;
                sellerNotesAttachement.FilePath = FilePath;
                sellerNotesAttachement.FileName = FileName;
                objMember.Entry(sellerNotesAttachement).State = EntityState.Modified;
                objMember.SaveChanges();

                TempData["Dashboard"] = userObj.FirstName + " " + userObj.LastName;
                if (submit == "Publish")
                {     
                    TempData["Message"] = "! Your Email has been Successfully Sent for Publish !";
                }
                else
                {
                    TempData["Message"] = "! Your Note has been Successfully Edited !";
                }
                return RedirectToAction("Dashboard", "Member");
            }

            ViewBag.Country = new SelectList(objMember.Countries.Where(x => x.IsActive == true), "CoutryID", "Name", objAddNote.Country);
            ViewBag.Category = new SelectList(objMember.NoteCategories.Where(x => x.IsActive == true), "CategoryID", "Name", objAddNote.Category);
            ViewBag.NoteType = new SelectList(objMember.NoteTypes.Where(x => x.IsActive == true), "TypeID ", "Name", objAddNote.NoteType);
            return View(objAddNote);
        }
        
        public ActionResult DownloadProfile(int? id)
        {
            UserProfile Profilepic = objMember.UserProfile.Where(x => x.UserID == id).FirstOrDefault();

            var displaypath = Profilepic.ProfilePicture;
            /*SellerNotes note = objMember.SellerNotes.Find(id);*/
            /* var allFilesPath = attechment.FilePath.Split(';');*/
            string FullPath = Path.Combine(Server.MapPath("~" + displaypath));
            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "image/*", FileName);

        }
        public ActionResult DownloadPicture(int? id)
        {
            SellerNotes note = objMember.SellerNotes.Find(id);

            var displaypath = note.DisplayPicture;

            /* var allFilesPath = attechment.FilePath.Split(';');*/

            string FullPath = Path.Combine(Server.MapPath("~" + displaypath));
            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "image/*", FileName);
        }

        public ActionResult DownloadPreview(int? id)
        {
            SellerNotes note = objMember.SellerNotes.Find(id);

            var Priviewpath = note.NotePreview;

            /* var allFilesPath = attechment.FilePath.Split(';');*/

            string FullPath = Path.Combine(Server.MapPath("~" + Priviewpath));
            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "application/pdf", FileName);
        }

        public ActionResult DownloadAttechedFile(int? id)
        {
            Downloads downloadnote = objMember.Downloads.Find(id);
            downloadnote.AttachmentDownloadedDate = DateTime.Now;          
            downloadnote.IsAttachmentDownloaded = true;
            downloadnote.ModifiedDate = DateTime.Now;
            objMember.Entry(downloadnote).State = EntityState.Modified;
            objMember.SaveChanges();

            SellerNotesAttachments attechment = objMember.SellerNotesAttachments.Where(x => x.NoteID == downloadnote.NoteID).FirstOrDefault();

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

        public ActionResult AttechedFile(int? id)
        {
            SellerNotesAttachments attechment = objMember.SellerNotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();

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

        [Route("DeletNote/{id}")]
        [HttpGet]
        public ActionResult DeletNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellerNotes sellerNote = objMember.SellerNotes.Find(id);
            if (sellerNote == null)
            {
                return HttpNotFound();
            }

            string storepath = Path.Combine(Server.MapPath("~/Members/" + sellerNote.SellerID), sellerNote.SellerNotesID.ToString());
            System.IO.DirectoryInfo di = new DirectoryInfo(storepath);
            // Check for Directory, If not exist, then create it  
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            objMember.SellerNotes.Remove(sellerNote);
            objMember.SaveChanges();

            var EmailID = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            TempData["Dashboard"] = userObj.FirstName + " " + userObj.LastName;
            TempData["Message"] = "! Your Note has been Successfully Deleted !";
            return RedirectToAction("Dashboard", "Member");

        }

        [Route("BuyerRequest")]
        public ActionResult BuyerRequest(int? page, string SearchNoteTitle, string SortOrder)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            UserProfile userprofiles = objMember.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
            if (userprofiles != null)
            {
                ViewBag.DateSortParm = String.IsNullOrEmpty(SortOrder) ? "Date_asc" : "";
                ViewBag.TitleSortParm = SortOrder == "Title" ? "Title_desc" : "Title";
                ViewBag.CategorySortParm = SortOrder == "Category" ? "Category_desc" : "Category";
                ViewBag.PriceSortParm = SortOrder == "Price" ? "Price_desc" : "Price";

                var BuyerRequest = objMember.Downloads.Where(x => x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true).Count();
                TempData["BuyerRequestCount"] = BuyerRequest;
                TempData.Keep("BuyerRequestCount");

                List<Downloads> Download = objMember.Downloads.Where(x =>x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true && (x.NoteTitle.Contains(SearchNoteTitle) || x.NoteCategory.Contains(SearchNoteTitle)||x.PurchasedPrice.ToString().StartsWith(SearchNoteTitle) || x.Users1.EmailID.Contains(SearchNoteTitle)||x.Users1.UserProfile.All(y=>y.PhoneNumber.Contains(SearchNoteTitle)) || SearchNoteTitle == null)).ToList();
                List<Users> user = objMember.Users.ToList();
                List<UserProfile> userprofile = objMember.UserProfile.ToList();
                /* (x.NoteTitle.Contains(SearchNoteTitle) || x.NoteCategory.Contains(SearchNoteTitle) || x.PurchasedPrice.ToString().StartsWith(SearchNoteTitle) || string.IsNullOrEmpty(SearchNoteTitle))*/
                
                var BuyerRequestsNote = (from nt in Download
                                         join cn in user on nt.Downloader equals cn.UserID into table1
                                         from cn in table1.ToList()
                                         join up in userprofile on nt.Downloader equals up.UserID into table2
                                         from up in table2.ToList()
                                         /*where(true &&( up.PhoneNumber.Contains(SearchNoteTitle) || SearchNoteTitle == null))*/
                                         select new BuyerRequest
                                         {
                                             DownloadNote = nt,
                                             BuyerDetail = cn,
                                             UserProfileNote = up
                                         }).AsQueryable();
                switch (SortOrder)
                {
                    case "Date_asc":
                        BuyerRequestsNote = BuyerRequestsNote.OrderBy(x => x.DownloadNote.CreatedDate);
                        break;
                    case "Title_desc":
                        BuyerRequestsNote = BuyerRequestsNote.OrderByDescending(x => x.DownloadNote.NoteTitle);
                        break;
                    case "Title":
                        BuyerRequestsNote = BuyerRequestsNote.OrderBy(x => x.DownloadNote.NoteTitle);
                        break;
                    case "Category_desc":
                        BuyerRequestsNote = BuyerRequestsNote.OrderByDescending(x => x.DownloadNote.NoteCategory);
                        break;
                    case "Category":
                        BuyerRequestsNote = BuyerRequestsNote.OrderBy(x => x.DownloadNote.NoteCategory);
                        break;
                    case "Price_desc":
                        BuyerRequestsNote = BuyerRequestsNote.OrderByDescending(x => x.DownloadNote.PurchasedPrice);
                        break;
                    case "Price":
                        BuyerRequestsNote = BuyerRequestsNote.OrderBy(x => x.DownloadNote.PurchasedPrice);
                        break;
                    default:
                        BuyerRequestsNote = BuyerRequestsNote.OrderByDescending(x => x.DownloadNote.CreatedDate);
                        break;
                }

                return View(BuyerRequestsNote.ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                var name = objMember.Users.Where(x => x.UserID == userObj.UserID).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                TempData["profileupdate"] = name;
                return RedirectToAction("UpdateProfile", "Member");
            }
        }

        [Route("AllowDownload/{id}")]
        public ActionResult AllowDownload(int? id)
        {
            Downloads AllowNote = objMember.Downloads.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotesAttachments selletNote = objMember.SellerNotesAttachments.Where(x => x.NoteID == AllowNote.NoteID).FirstOrDefault();
            if (selletNote == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            bool internet = CheckInternet.IsConnectedToInternet();
            if (internet != true)
            {
                TempData["internetnotconnected"] = "user";
                return RedirectToAction("Dashboard", "Member");
            }

            AllowNote.IsSellerHasAllowedDownload = true;
            AllowNote.AttachmentPath = selletNote.FilePath;
            AllowNote.CreatedDate = DateTime.Now;
            AllowNote.ModifiedDate = DateTime.Now;
            AllowNote.ModifiedBy = AllowNote.Seller;

            objMember.Entry(AllowNote).State = EntityState.Modified;
            objMember.SaveChanges();

            Users SellerDetail = objMember.Users.Find(AllowNote.Seller);
            Users BuyerDetail = objMember.Users.Find(AllowNote.Downloader);

            var emailid = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
            var password = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();

            AllowDownloadEmail.AlloWDownloadNotifyEmail(SellerDetail, BuyerDetail,emailid,password);
            TempData["AllowDownload"] = BuyerDetail.EmailID;
            TempData["title"] = AllowNote.NoteTitle;
            return RedirectToAction("BuyerRequest", "Member");
        }

        [Route("MyDownloads")]
        public ActionResult MyDownloads(int? page, string SearchNoteTitle, string SortOrder)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.DateSortParm = String.IsNullOrEmpty(SortOrder) ? "Date_asc" : "";
            ViewBag.TitleSortParm = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellTypeSortParm = SortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = SortOrder == "Price" ? "Price_desc" : "Price";

            List<Downloads> Download = objMember.Downloads.Where(x => x.Downloader == userObj.UserID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null && (x.NoteTitle.Contains(SearchNoteTitle) || x.NoteCategory.Contains(SearchNoteTitle) || x.PurchasedPrice.ToString().StartsWith(SearchNoteTitle)||x.Users.EmailID.Contains(SearchNoteTitle) || SearchNoteTitle == null)).ToList();
            List<Users> user = objMember.Users.ToList();
            /*List<UserProfile> userprofile = objMember.UserProfile.ToList();*/
            /* (x.NoteTitle.Contains(SearchNoteTitle) || x.NoteCategory.Contains(SearchNoteTitle) || x.PurchasedPrice.ToString().StartsWith(SearchNoteTitle) || string.IsNullOrEmpty(SearchNoteTitle))*/

            var MyDownloadsNote = (from nt in Download
                                   join cn in user on nt.Downloader equals cn.UserID into table1
                                   from cn in table1.ToList()
                                   join sd in user on nt.Seller equals sd.UserID into table2
                                   from sd in table2.ToList()
                                   select new MyDownloadNote
                                   {
                                       DownloadNote = nt,
                                       BuyerDetail = cn,
                                       SellerDetail = sd
                                   }).AsQueryable();

            ViewBag.seller = objMember.Users.ToList();

            switch (SortOrder)
            {
                case "Date_asc":
                    MyDownloadsNote = MyDownloadsNote.OrderBy(x => x.DownloadNote.AttachmentDownloadedDate);
                    break;
                case "Title_desc":
                    MyDownloadsNote = MyDownloadsNote.OrderByDescending(x => x.DownloadNote.NoteTitle);
                    break;
                case "Title":
                    MyDownloadsNote = MyDownloadsNote.OrderBy(x => x.DownloadNote.NoteTitle);
                    break;
                case "Category_desc":
                    MyDownloadsNote = MyDownloadsNote.OrderByDescending(x => x.DownloadNote.NoteCategory);
                    break;
                case "Category":
                    MyDownloadsNote = MyDownloadsNote.OrderBy(x => x.DownloadNote.NoteCategory);
                    break;
                case "SellType_desc":
                    MyDownloadsNote = MyDownloadsNote.OrderByDescending(x => x.DownloadNote.IsPaid);
                    break;
                case "SellType":
                    MyDownloadsNote = MyDownloadsNote.OrderBy(x => x.DownloadNote.IsPaid);
                    break;
                case "Price_desc":
                    MyDownloadsNote = MyDownloadsNote.OrderByDescending(x => x.DownloadNote.PurchasedPrice);
                    break;
                case "Price":
                    MyDownloadsNote = MyDownloadsNote.OrderBy(x => x.DownloadNote.PurchasedPrice);
                    break;
                default:
                    MyDownloadsNote = MyDownloadsNote.OrderByDescending(x => x.DownloadNote.AttachmentDownloadedDate);
                    break;
            }

            return View(MyDownloadsNote.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("ReportedIssues/{id}")]
        [HttpGet]
        public ActionResult RepotedIssues(int? id,ReportedIssues reportissues)
        {
            var Emailid = User.Identity.Name.ToString();
            Users user = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           /* Downloads note = objMember.Downloads.Where(x=>x.DownloadsID == id).FirstOrDefault();*/
            Downloads note = objMember.Downloads.Find(id);
            if (note == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            bool internet = CheckInternet.IsConnectedToInternet();
            if (internet != true)
            {
                TempData["internetnotconnected"] = user.FirstName+" "+user.LastName;
                return RedirectToAction("Dashboard", "Member");
            }
                SellerNotesReportedIssues issues = new SellerNotesReportedIssues {
                NoteID = note.NoteID,
                ReportedBy = user.UserID,
                AgaintsDownloadID = note.DownloadsID,
                Remarks = reportissues.Remarks,
                CreatedBy = user.UserID,
                CreatedDate = DateTime.Now,
                IsActive = true,
                ModifiedBy = user.UserID,
                ModifiedDate = DateTime.Now 
            };
            objMember.SellerNotesReportedIssues.Add(issues);
            objMember.SaveChanges();

            var title = note.NoteTitle;

            Users sellername = objMember.Users.Where(x => x.UserID == note.Seller).FirstOrDefault();
            var emailid = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
            var password = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
            var emails = objMember.SystemConfigurations.Where(x => x.keys.ToLower() == "emailids").Select(x => x.Value).FirstOrDefault();
            ReportedIssuesEmail.ReportedIssuesNoteNotify(sellername, user, title,emailid,password,emails);
            TempData["MyDownloads"] = user.FirstName+" "+user.LastName;
            TempData["Message"] = "Your Email Successfully sent. For Reported As Inapropriate on ";
            TempData["title"] =note.NoteTitle;

            return RedirectToAction("MyDownloads","Member");
        }

        [Route("NoteReview/{id}")]
        [HttpGet]
        public ActionResult NoteReview(int? id, NoteReview notereview)
        {
            var Emailid = User.Identity.Name.ToString();
            Users user = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            /* Downloads note = objMember.Downloads.Where(x=>x.DownloadsID == id).FirstOrDefault();*/
            Downloads note = objMember.Downloads.Find(id);
            if (note == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            SellerNotesReviews review = new SellerNotesReviews
            {
                NoteID = note.NoteID,
                ReviewedByID = user.UserID,
                AgaintsDownloadsID = note.DownloadsID,
                Comments = notereview.Comments,
                Ratings = notereview.Ratings,
                CreatedBy = user.UserID,
                CreatedDate = DateTime.Now,
                IsActive = true,
                ModifiedBy = user.UserID,
                ModifiedDate = DateTime.Now
            };
            objMember.SellerNotesReviews.Add(review);
            objMember.SaveChanges();

            TempData["MyDownloads"] = user.FirstName +" "+user.LastName;
            TempData["Message"] = "Your Review has been Successfully Added to";
            TempData["title"] = note.NoteTitle;
            return RedirectToAction("MyDownloads", "Member");
        }

        [Route("MySoldNotes")]
        public ActionResult MySoldNotes(int? page, string SearchNoteTitle, string SortOrder)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.DateSortParm = String.IsNullOrEmpty(SortOrder) ? "Date_asc" : "";
            ViewBag.TitleSortParm = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellTypeSortParm = SortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = SortOrder == "Price" ? "Price_desc" : "Price";

            List<Downloads> Download = objMember.Downloads.Where(x => x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null && (x.NoteTitle.Contains(SearchNoteTitle) || x.NoteCategory.Contains(SearchNoteTitle) || x.PurchasedPrice.ToString().StartsWith(SearchNoteTitle)||x.Users1.EmailID.Contains(SearchNoteTitle) || SearchNoteTitle == null)).ToList();
            List<Users> user = objMember.Users.ToList();
            /*List<UserProfile> userprofile = objMember.UserProfile.ToList();*/
            /* (x.NoteTitle.Contains(SearchNoteTitle) || x.NoteCategory.Contains(SearchNoteTitle) || x.PurchasedPrice.ToString().StartsWith(SearchNoteTitle) || string.IsNullOrEmpty(SearchNoteTitle))*/

            var MySoldsNote = (from nt in Download
                                   join cn in user on nt.Downloader equals cn.UserID into table1
                                   from cn in table1.ToList()
                                   select new MySoldNote
                                   {
                                       DownloadNote = nt,
                                       BuyerDetail = cn,

                                   }).AsQueryable();


            switch (SortOrder)
            {
                case "Date_asc":
                    MySoldsNote = MySoldsNote.OrderBy(x => x.DownloadNote.CreatedDate);
                    break;
                case "Title_desc":
                    MySoldsNote = MySoldsNote.OrderByDescending(x => x.DownloadNote.NoteTitle);
                    break;
                case "Title":
                    MySoldsNote = MySoldsNote.OrderBy(x => x.DownloadNote.NoteTitle);
                    break;
                case "Category_desc":
                    MySoldsNote = MySoldsNote.OrderByDescending(x => x.DownloadNote.NoteCategory);
                    break;
                case "Category":
                    MySoldsNote = MySoldsNote.OrderBy(x => x.DownloadNote.NoteCategory);
                    break;
                case "SellType_desc":
                    MySoldsNote = MySoldsNote.OrderByDescending(x => x.DownloadNote.IsPaid);
                    break;
                case "SellType":
                    MySoldsNote = MySoldsNote.OrderBy(x => x.DownloadNote.IsPaid);
                    break;
                case "Price_desc":
                    MySoldsNote = MySoldsNote.OrderByDescending(x => x.DownloadNote.PurchasedPrice);
                    break;
                case "Price":
                    MySoldsNote = MySoldsNote.OrderBy(x => x.DownloadNote.PurchasedPrice);
                    break;
                default:
                    MySoldsNote = MySoldsNote.OrderByDescending(x => x.DownloadNote.CreatedDate);
                    break;
            }

            return View(MySoldsNote.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("MyRejectedNote")]
        public ActionResult MyRejectedNote(int? page, string SearchNoteTitle, string SortOrderProgress)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objMember.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.DateSortParm = String.IsNullOrEmpty(SortOrderProgress) ? "Date_asc" : "";
            ViewBag.TitleSortParm = SortOrderProgress == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = SortOrderProgress == "Category" ? "Category_desc" : "Category";
            

            List<SellerNotes> NoteTitle = objMember.SellerNotes.Where(x => x.SellerID == userObj.UserID && x.IsActive == true && (x.Title.Contains(SearchNoteTitle)||x.NoteCategories.Name.Contains(SearchNoteTitle) || x.AdminRemarks.Contains(SearchNoteTitle) || SearchNoteTitle == null)).ToList();
            List<NoteCategories> CategoryName = objMember.NoteCategories.ToList();
            List<ReferenceData> StatusName = objMember.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Removed" && x.IsActive == true).ToList();

            var MyRejectedNote = (from nt in NoteTitle
                                join cn in CategoryName on nt.Category equals cn.CategoryID into table1
                                from cn in table1.ToList()
                                join sn in StatusName on nt.Status equals sn.ReferenceID into table2
                                from sn in table2.ToList()
                                where sn.Value == "Rejected"
                                select new RejectedNote
                                {
                                    NoteDetails = nt,
                                    Category = cn,
                                    status = sn
                                }).AsQueryable();

            switch (SortOrderProgress)
            {
                case "Date_asc":
                    MyRejectedNote = MyRejectedNote.OrderBy(x => x.NoteDetails.ModifiedDate);
                    break;
                case "Title_desc":
                    MyRejectedNote = MyRejectedNote.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    MyRejectedNote = MyRejectedNote.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    MyRejectedNote = MyRejectedNote.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    MyRejectedNote = MyRejectedNote.OrderBy(x => x.Category.Name);
                    break;    
                default:
                    MyRejectedNote = MyRejectedNote.OrderByDescending(x => x.NoteDetails.ModifiedDate);
                    break;
            }
            ViewBag.MyRejectedNote = MyRejectedNote.ToList().ToPagedList(page ?? 1, 4);

            return View();
        }

        [Route("Clone/{id}")]
        public ActionResult Clone(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Emailid = User.Identity.Name.ToString();
            Users user = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            SellerNotes note = objMember.SellerNotes.Find(id);

            if (note == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            note.Status = objMember.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value == "Draft").Select(x => x.ReferenceID).FirstOrDefault();
            note.AdminRemarks = null;
            note.ModifiedBy = user.UserID;
            note.ModifiedDate = DateTime.Now;
          
            objMember.Entry(note).State = EntityState.Modified;
            objMember.SaveChanges();

            TempData["RejectedNote"] = user.FirstName + " " + user.LastName;
            TempData["Message"] = "! Your Clone Request has been Successfully !";
            
            return RedirectToAction("MyRejectedNote", "Member");

        }

        [Route("ChangePassword")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.Title = "NotesMarketPlace";
            return View();
        }

        [Route("ChangePassword")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePwd)
        {
            if (ModelState.IsValid)
            {


                if (User.Identity.IsAuthenticated)
                {
                    var Emailid = User.Identity.Name.ToString();

                    Users user = objMember.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                    var pwd = EncryptPassword.EncryptPasswordMd5(changePwd.OldPassword);

                    if (user.Password == pwd)
                    {
                        user.Password = EncryptPassword.EncryptPasswordMd5(changePwd.NewPassword);
                        user.ModifiedDate = DateTime.Now;
                        user.ModifiedBy = user.UserID;            

                        objMember.Entry(user).State = EntityState.Modified;
                        objMember.SaveChanges();

                        return RedirectToAction("Dashboard", "Member");
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