using NotesMarketPlace.Models;
using NotesMarketPlace.ViewModel;
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
                ContactUsEmail.ContactEmail(contact);

                if (v != null)
                {
                   
                    return RedirectToAction("Dashboard", "Member");
                }
                else
                {
                   
                    return RedirectToAction("Index","HomePage");
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
        public ActionResult SearchNotesPage(string SearchText,string NoteType,string Category,string University, string Course,string Country, int? page)
        {
            var EmailID = User.Identity.Name.ToString();
            Users userObj = dbDash.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = dbDash.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"]= Path.Combine("/SystemConfiguration/DefaultImage/", "DefaultUserImage.jpg");
                }
            }

            List<SellerNotes> NoteTitle = dbDash.SellerNotes.OrderBy(x=>x.Title).Where(x=>x.IsActive == true && (x.Title.StartsWith(SearchText) || SearchText == null)).ToList();
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
                                  where (sn.Value == "Published" && ((nt.Category.ToString() == Category || string.IsNullOrEmpty(Category)) &&
                                  (nt.NoteType.ToString() == NoteType || string.IsNullOrEmpty(NoteType))
                                  && (nt.Country.ToString() == Country || string.IsNullOrEmpty(Country))
                                  && (nt.UniversityName == University || string.IsNullOrEmpty(University))
                                  && (nt.Course == Course || string.IsNullOrEmpty(Course))))
                                  select new AllSearchNotes
                                  {
                                      NoteDetails = nt,
                                      Category = cn,
                                      status = sn,
                                      Country = co,                                                
                                };


            ViewBag.TotalNotesRecord = AllNotesRecords.Count();
            ViewBag.Ratings = dbDash.SellerNotesReviews.Select(x => new { x.Ratings }).Distinct().ToList();
            ViewBag.Course = dbDash.SellerNotes.Select(x => new { x.Course}).Distinct().Where(x=>x.Course != null).ToList();
            ViewBag.University = dbDash.SellerNotes.Select(x => new { x.UniversityName }).Distinct().Where(x => x.UniversityName != null).ToList();
            ViewBag.Country = new SelectList(dbDash.Countries, "CoutryID", "Name");
            ViewBag.Category = new SelectList(dbDash.NoteCategories, "CategoryID", "Name");
            ViewBag.NoteType = new SelectList(dbDash.NoteTypes, "TypeID", "Name");   
          
            return View(AllNotesRecords.ToList().ToPagedList(page ?? 1,3));

        }
        
        [Route("NoteDetails/{id}")]
        public ActionResult NoteDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotes sellerNote = dbDash.SellerNotes.Find(id);

            ViewBag.CustomerReview = dbDash.Users.Where(x => x.UserID == sellerNote.SellerNotesID).Select(x=>x.FirstName);
           
            if (sellerNote == null)
            {
                return HttpNotFound();
            }
            Countries country = dbDash.Countries.Find(sellerNote.Country);
            NoteCategories category = dbDash.NoteCategories.Find(sellerNote.Category);
            SellerNotesAttachments attechment = dbDash.SellerNotesAttachments.Where(x => x.NoteID == sellerNote.SellerNotesID).FirstOrDefault();
            if(country == null)
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
                return HttpNotFound();
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

                return RedirectToAction("Dashboard", "Member");
            }
            else
            {

                Downloads download = new Downloads
                {
                    NoteID = sellerNote.SellerNotesID,
                    Seller = sellerNote.SellerID,
                    Downloader = userObj.UserID,
                    IsSellerHasAllowedDownload = false,
                    AttachmentPath = null,
                    IsAttachmentDownloaded = false,
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

                Users sellerRecord = dbDash.Users.Find(sellerNote.SellerID);
                /*var sellerEmail = sellerRecord.EmailID;*/

                BuyerRequestEmail.BuyerNotifyEmail(userObj, sellerRecord);

            }



            return RedirectToAction("Dashboard","Member");
        }

        
    }
}