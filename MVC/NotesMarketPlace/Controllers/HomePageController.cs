using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.IO;
using System.IO.Compression;
using NotesMarketPlace.EmailTemplates;
using System.Net.NetworkInformation;
using NotesMarketPlace.Healpers;

namespace NotesMarketPlace.Controllers
{
    [RoutePrefix("HomePage")]
    public class HomePageController : Controller
    {
        private NotesMarketPlaceEntities dbDash = new NotesMarketPlaceEntities();

        // GET: HomePage
        /*[Route("HomePage/Index")]*/
        public ActionResult Index()
        {
            var Records = dbDash.SystemConfigurations.Count();
            if (Records > 0)
            {
                TempData["FBURL"] = dbDash.SystemConfigurations.Where(x => x.keys == "FBURL").Select(x => x.Value).FirstOrDefault();
                TempData["LinkedInURL"] = dbDash.SystemConfigurations.Where(x => x.keys == "LinkedInURL").Select(x => x.Value).FirstOrDefault();
                TempData["TwitterURL"] = dbDash.SystemConfigurations.Where(x => x.keys == "TwitterURL").Select(x => x.Value).FirstOrDefault();
                TempData.Keep("FBURL");
                TempData.Keep("LinkedInURL");
                TempData.Keep("TwitterURL");
            }
            return View();
        }
        [Route("ContactUs")]
        [HttpGet]
        public ActionResult ContactUs()
        {
           
            ViewBag.Title = "ContactUs";
            var Emailid = User.Identity.Name.ToString();

            var v = dbDash.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
            if (v != null)
            {
                ContactUs contact = new ContactUs {
                    FullName = v.FirstName +" "+ v.LastName,
                    EmailID = v.EmailID
                };
                return View(contact);
                
            }
            return View();
        }

        [Route("ContactUs")]
        [HttpPost]
        public ActionResult ContactUs(ContactUs contactus)
        {
            if (ModelState.IsValid)
            {
                var EmailIDs = User.Identity.Name.ToString();

                var v = dbDash.Users.Where(x => x.EmailID == EmailIDs).FirstOrDefault();

                ContactUs contact = new ContactUs
                {
                    FullName = contactus.FullName,
                    EmailID = contactus.EmailID,
                    Subject = contactus.Subject,
                    Comments = contactus.Comments
                };
                bool internet = CheckInternet.IsConnectedToInternet();
                if (internet == true)
                {
                    var emailid = dbDash.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
                    var password = dbDash.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
                    var emails = dbDash.SystemConfigurations.Where(x => x.keys.ToLower() == "emailids").Select(x => x.Value).FirstOrDefault();
                    ContactUsEmail.ContactEmail(contact,emailid,password,emails);

                    if (v != null)
                    {
                        Users userObj = dbDash.Users.Where(x => x.EmailID == EmailIDs).FirstOrDefault();
                        TempData["Dashboard"] = userObj.FirstName + " " + userObj.LastName;
                        TempData["Message"] = "! Your Email has been Successfully Sent !";
                        return RedirectToAction("Dashboard", "Member");
                    }
                    else
                    {
                        TempData["Dashboard"] = contactus.FullName;
                        TempData["Message"] = "! Your Email has been Successfully Sent !";
                        return RedirectToAction("Index", "HomePage");
                    }
                }
                else
                {
                    TempData["internetnotconnected"] = contactus.FullName;
                    return RedirectToAction("Dashboard", "Member");
                }

            }
            return View();
        }

        [Route("FAQ")]
        public ActionResult FAQ()
        {
            return View();
        }

        [Route("SearchNotesPage")]
        public ActionResult SearchNotesPage(string SearchText,string NoteType,string Category,string University, string Course,string Country,string Ratings , int? page)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = dbDash.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            if (User.Identity.IsAuthenticated)
            {
                var BuyerRequest = dbDash.Downloads.Where(x => x.Seller == userObj.UserID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true).Count();
                TempData["BuyerRequestCount"] = BuyerRequest;
                TempData.Keep("BuyerRequestCount");
            }
            var Records = dbDash.SystemConfigurations.Count();
            if (Records > 0)
            {
                TempData["FBURL"] = dbDash.SystemConfigurations.Where(x => x.keys == "FBURL").Select(x => x.Value).FirstOrDefault();
                TempData["LinkedInURL"] = dbDash.SystemConfigurations.Where(x => x.keys == "LinkedInURL").Select(x => x.Value).FirstOrDefault();
                TempData["TwitterURL"] = dbDash.SystemConfigurations.Where(x => x.keys == "TwitterURL").Select(x => x.Value).FirstOrDefault();
                TempData.Keep("FBURL");
                TempData.Keep("LinkedInURL");
                TempData.Keep("TwitterURL");
            }

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = dbDash.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"]= dbDash.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault();
                }
            }

            List<SellerNotes> NoteTitle = dbDash.SellerNotes.OrderBy(x=>x.Title).Where(x=>x.IsActive == true && (x.Title.Contains(SearchText)||x.NoteCategories.Name.Contains(SearchText) || SearchText == null)).ToList();
            List<NoteCategories> CategoryName = dbDash.NoteCategories.ToList();
            List<ReferenceData> StatusName = dbDash.ReferenceData.ToList();
            List<Countries> CountryName = dbDash.Countries.ToList();
            
                    
            var AllNotesRecords = from nt in NoteTitle
                                  join cn in CategoryName on nt.Category equals cn.CategoryID into table1
                                  from cn in table1.ToList()
                                  join sn in StatusName on nt.Status equals sn.ReferenceID into table2
                                  from sn in table2.ToList().DefaultIfEmpty()
                                  join co in CountryName on nt.Country equals co.CoutryID into table3
                                  from co in table3.ToList().DefaultIfEmpty()   
                                  where (sn.Value == "Published" && ((nt.Category.ToString() == Category || string.IsNullOrEmpty(Category))
                                  && (nt.NoteType.ToString() == NoteType || string.IsNullOrEmpty(NoteType))
                                  && (nt.Country.ToString() == Country || string.IsNullOrEmpty(Country))
                                  && (nt.UniversityName == University || string.IsNullOrEmpty(University))
                                 && (nt.Course == Course || string.IsNullOrEmpty(Course))
                                 && ((nt.SellerNotesReviews.Count() == 0 ? 0 : Math.Round(nt.SellerNotesReviews.Where(x=>x.NoteID == nt.SellerNotesID).Sum(r => r.Ratings) / nt.SellerNotesReviews.Where(x => x.NoteID == nt.SellerNotesID).Count())).ToString() == Ratings || String.IsNullOrEmpty(Ratings))))
                                  select new AllSearchNotes
                                  {
                                      NoteDetails = nt,
                                      Category = cn,
                                      status = sn,
                                      Country = co,                                                
                                };


            ViewBag.TotalNotesRecord = AllNotesRecords.Count();
            ViewBag.Ratings = Enumerable.Range(0,6).ToList();
            ViewBag.Course = dbDash.SellerNotes.Select(x => new { x.Course}).Distinct().Where(x=>x.Course != null).ToList();
            ViewBag.University = dbDash.SellerNotes.Select(x => new { x.UniversityName }).Distinct().Where(x => x.UniversityName != null).ToList();
            ViewBag.Country = new SelectList(dbDash.Countries, "CoutryID", "Name");
            ViewBag.Category = new SelectList(dbDash.NoteCategories, "CategoryID", "Name");
            ViewBag.NoteType = new SelectList(dbDash.NoteTypes, "TypeID", "Name");   
          
            return View(AllNotesRecords.ToList().ToPagedList(page ?? 1,9));

        }
        
        [Route("NoteDetails/{id}")]
        public ActionResult NoteDetails(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SellerNotes sellerNote = dbDash.SellerNotes.Find(id);

                ViewBag.CustomerReview = dbDash.Users.Where(x => x.UserID == sellerNote.SellerNotesID);

                if (sellerNote == null)
                {
                return RedirectToAction("Error", "HomePage");
            }
                Countries country = dbDash.Countries.Find(sellerNote.Country);
                NoteCategories category = dbDash.NoteCategories.Find(sellerNote.Category);
                SellerNotesAttachments attechment = dbDash.SellerNotesAttachments.Where(x => x.NoteID == sellerNote.SellerNotesID).FirstOrDefault();
                if (country == null)
                {
                    ViewBag.Country = null;
                }
                else
                {
                    ViewBag.Country = country.Name;
                }
                var EmailID = User.Identity.Name.ToString();
                Users userObj = dbDash.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.LoginUserid = userObj.UserID;
            }
                ViewBag.Name = userObj;
                ViewBag.seller = dbDash.Users.Where(x => x.UserID == sellerNote.SellerID).Select(y => y.FirstName + " " + y.LastName).FirstOrDefault();
                ViewBag.sellerNumber = dbDash.UserProfile.Where(x => x.UserID == sellerNote.SellerID).Select(y => y.PhoneNumberCountryCode + " " + y.PhoneNumber).FirstOrDefault();
                ViewBag.Category = category.Name;
                ViewBag.Attchment = attechment.FilePath;

                ViewBag.publishedid = dbDash.ReferenceData.Where(x => x.Value == "Published").Select(x => x.ReferenceID).FirstOrDefault();

                return View(sellerNote);
           

        }

        [Authorize]
        [Route("HomePage/DownloadNote/{id}")]
        [HttpPost]
        public ActionResult DownloadNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotes sellerNote = dbDash.SellerNotes.Find(id);

            if (sellerNote == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            var EmailID = User.Identity.Name.ToString();
            Users userObj = dbDash.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            SellerNotesAttachments attechment = dbDash.SellerNotesAttachments.Where(x=>x.NoteID == sellerNote.SellerNotesID).FirstOrDefault();
            NoteCategories category = dbDash.NoteCategories.Where(x => x.CategoryID == sellerNote.Category).FirstOrDefault(); ;
            if (sellerNote.IsPaid == false)
            {
                Downloads download = new Downloads
                {
                    NoteID = sellerNote.SellerNotesID,
                    Seller = sellerNote.SellerID,
                    Downloader = userObj.UserID,
                    IsSellerHasAllowedDownload = true,
                    AttachmentPath = attechment.FilePath,
                    IsAttachmentDownloaded = true,
                    AttachmentDownloadedDate = DateTime.Now, 
                    IsPaid = sellerNote.IsPaid,
                    PurchasedPrice = sellerNote.SellingPrice,
                    NoteTitle = sellerNote.Title,
                    NoteCategory = category.Name,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userObj.UserID,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = userObj.UserID,
                     IsActive = true
                };

                dbDash.Downloads.Add(download);
                dbDash.SaveChanges();

                var allFilesPath = attechment.FilePath.Split(';');
                var allFileName = attechment.FileName.Split(';');

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

                /*return RedirectToAction("Dashboard", "Member");*/
            }
            else
            {
               
                bool internet = CheckInternet.IsConnectedToInternet();
                if (internet == true)
        {

            Downloads download = new Downloads
            {
                NoteID = sellerNote.SellerNotesID,
                Seller = sellerNote.SellerID,
                Downloader = userObj.UserID,
                IsSellerHasAllowedDownload = false,
                AttachmentPath = null,
                IsAttachmentDownloaded = false,
                IsPaid = sellerNote.IsPaid,
                PurchasedPrice = sellerNote.SellingPrice,
                NoteTitle = sellerNote.Title,
                NoteCategory = category.Name,
                CreatedDate = DateTime.Now,
                CreatedBy = userObj.UserID,
                ModifiedDate = DateTime.Now,
                ModifiedBy = userObj.UserID,
                IsActive = true
            };

            dbDash.Downloads.Add(download);
            dbDash.SaveChanges();

            Users sellerRecord = dbDash.Users.Find(sellerNote.SellerID);
            /*var sellerEmail = sellerRecord.EmailID;*/
             var emailid = dbDash.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
            var password = dbDash.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
            BuyerRequestEmail.BuyerNotifyEmail(userObj, sellerRecord,emailid,password);
            
            TempData["Dashboard"] = userObj.FirstName +" "+userObj.LastName;
            TempData["Message"] = "! Your Request has been Successfully sent!";

                }
                else
            {      

                TempData["internetnotconnected"] = userObj.FirstName +" "+userObj.LastName;
                return RedirectToAction("Dashboard", "Member");
            }

        }
          
            return RedirectToAction("Dashboard","Member");
        }

        public ActionResult Error()
        {
            return View();
        }


    }
}