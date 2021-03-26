using NotesMarketPlace.EmailTemplates;
using NotesMarketPlace.Healpers;
using NotesMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminSettingsController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

        [Route("SystemConfiguration")]
        [HttpGet]
        public ActionResult SystemConfiguration()
        {
            /* var sc = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportEmailID");*/
            var Records = objAdmin.SystemConfigurations.Count();
            if (Records > 0)
            {
                SystemConfigurationSuperAdmin SA = new SystemConfigurationSuperAdmin
                {
                    SupportEmailID = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportEmailID").Select(y => y.Value).FirstOrDefault(),
                    SupportEmailIDPassword = objAdmin.SystemConfigurations.Where(x=>x.keys == "SupportEmailIDPassword").Select(y=>y.Value).FirstOrDefault(),
                    SupportPhoneNumber = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportPhoneNumber").Select(y => y.Value).FirstOrDefault(),
                    EmailIDs = objAdmin.SystemConfigurations.Where(x => x.keys == "EmailIDs").Select(y => y.Value).FirstOrDefault(),
                    FBURL = objAdmin.SystemConfigurations.Where(x => x.keys == "FBURL").Select(y => y.Value).FirstOrDefault(),
                    LinkedInURL = objAdmin.SystemConfigurations.Where(x => x.keys == "LinkedInURL").Select(y => y.Value).FirstOrDefault(),
                    TwitterURL = objAdmin.SystemConfigurations.Where(x => x.keys == "TwitterURL").Select(y => y.Value).FirstOrDefault(),
                };
                return View(SA);
            }
            return View();
        }

        [Route("SystemConfiguration")]
        [HttpPost]
        public ActionResult SystemConfiguration(SystemConfigurationSuperAdmin sc)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
                /*var SA = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportEmailID");*/
                var Records = objAdmin.SystemConfigurations.Count();
                if (Records > 0)
                {
                    SystemConfigurations updatedetail1 = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportEmailID").FirstOrDefault();
                    updatedetail1.Value = sc.SupportEmailID;
                    updatedetail1.ModofiedBy = userObj.UserID;
                    updatedetail1.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail1).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail9 = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportEmailIDPassword").FirstOrDefault();
                    updatedetail9.Value = sc.SupportEmailIDPassword;
                    updatedetail9.ModofiedBy = userObj.UserID;
                    updatedetail9.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail9).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail2 = objAdmin.SystemConfigurations.Where(x => x.keys == "SupportPhoneNumber").FirstOrDefault();
                    updatedetail2.Value = sc.SupportPhoneNumber;
                    updatedetail2.ModofiedBy = userObj.UserID;
                    updatedetail2.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail2).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail3 = objAdmin.SystemConfigurations.Where(x => x.keys == "EmailIDs").FirstOrDefault();
                    updatedetail3.Value = sc.EmailIDs;
                    updatedetail3.ModofiedBy = userObj.UserID;
                    updatedetail3.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail3).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail4 = objAdmin.SystemConfigurations.Where(x => x.keys == "FBURL").FirstOrDefault();
                    updatedetail4.Value = sc.FBURL;
                    updatedetail4.ModofiedBy = userObj.UserID;
                    updatedetail4.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail4).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail5 = objAdmin.SystemConfigurations.Where(x => x.keys == "LinkedInURL").FirstOrDefault();
                    updatedetail5.Value = sc.LinkedInURL;
                    updatedetail5.ModofiedBy = userObj.UserID;
                    updatedetail5.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail5).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail6 = objAdmin.SystemConfigurations.Where(x => x.keys == "TwitterURL").FirstOrDefault();
                    updatedetail6.Value = sc.TwitterURL;
                    updatedetail6.ModofiedBy = userObj.UserID;
                    updatedetail6.ModofiedDate = DateTime.Now;
                    objAdmin.Entry(updatedetail6).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail7 = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultNotePicture").FirstOrDefault();
                    updatedetail7.ModofiedBy = userObj.UserID;
                    updatedetail7.ModofiedDate = DateTime.Now;
                    string storepath = Path.Combine(Server.MapPath("~/SystemConfigurations/"), "DefaultImages");
                    System.IO.DirectoryInfo di = new DirectoryInfo(storepath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    if (sc.DefaultNotePicture != null && sc.DefaultNotePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sc.DefaultNotePicture.FileName);
                        string extension = Path.GetExtension(sc.DefaultNotePicture.FileName);
                        fileName = "DefaultNoteImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        sc.DefaultNotePicture.SaveAs(finalpath);

                        updatedetail7.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }
                    objAdmin.Entry(updatedetail7).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    SystemConfigurations updatedetail8 = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").FirstOrDefault();
                    updatedetail8.ModofiedBy = userObj.UserID;
                    updatedetail8.ModofiedDate = DateTime.Now;
                    if (sc.DefaultUserPicture != null && sc.DefaultUserPicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sc.DefaultUserPicture.FileName);
                        string extension = Path.GetExtension(sc.DefaultUserPicture.FileName);
                        fileName = "DefaultUserImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        sc.DefaultUserPicture.SaveAs(finalpath);

                        updatedetail8.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }
                    objAdmin.Entry(updatedetail8).State = EntityState.Modified;
                    objAdmin.SaveChanges();

                    TempData["AdminDashboard"] = userObj.FirstName + " " + userObj.LastName;
                    TempData["Message"] = "System Configuration has been Successfully Edited !";

                }
                else
                {
                 
                    SystemConfigurations obj1 = new SystemConfigurations
                    {
                        keys = "SupportEmailID",
                        Value = sc.SupportEmailID,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj1);
                    objAdmin.SaveChanges();
                    SystemConfigurations obj9 = new SystemConfigurations
                    {
                        keys = "SupportEmailIDPassword",
                        Value = sc.SupportEmailIDPassword,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj9);
                    objAdmin.SaveChanges();
                    SystemConfigurations obj2 = new SystemConfigurations
                    {
                        keys = "SupportPhoneNumber",
                        Value = sc.SupportPhoneNumber,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj2);
                    objAdmin.SaveChanges();
                    SystemConfigurations obj3 = new SystemConfigurations
                    {
                        keys = "EmailIDs",
                        Value = sc.EmailIDs,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj3);
                    objAdmin.SaveChanges();
                    SystemConfigurations obj4 = new SystemConfigurations
                    {
                        keys = "FBURL",
                        Value = sc.FBURL,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj4);
                    objAdmin.SaveChanges();
                    SystemConfigurations obj5 = new SystemConfigurations
                    {
                        keys = "LinkedInURL",
                        Value = sc.LinkedInURL,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj5);
                    objAdmin.SaveChanges();
                    SystemConfigurations obj6 = new SystemConfigurations
                    {
                        keys = "TwitterURL",
                        Value = sc.TwitterURL,
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    objAdmin.SystemConfigurations.Add(obj6);
                    objAdmin.SaveChanges();

                    SystemConfigurations obj7 = new SystemConfigurations
                    {
                        keys = "DefaultNotePicture",
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };
                    

                    string storepath = Path.Combine(Server.MapPath("~/SystemConfigurations/"),"DefaultImages");

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    if (sc.DefaultNotePicture != null && sc.DefaultNotePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sc.DefaultNotePicture.FileName);
                        string extension = Path.GetExtension(sc.DefaultNotePicture.FileName);
                        fileName = "DefaultNoteImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        sc.DefaultNotePicture.SaveAs(finalpath);

                        obj7.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }

                    objAdmin.SystemConfigurations.Add(obj7);
                    objAdmin.SaveChanges();

                    SystemConfigurations obj8 = new SystemConfigurations
                    {
                        keys = "DefaultUserPicture",
                        CreatedDate = DateTime.Now,
                        ModofiedDate = DateTime.Now,
                        CreatedBy = userObj.UserID,
                        ModofiedBy = userObj.UserID,
                        IsActive = true
                    };

                    if (sc.DefaultUserPicture != null && sc.DefaultUserPicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sc.DefaultUserPicture.FileName);
                        string extension = Path.GetExtension(sc.DefaultUserPicture.FileName);
                        fileName = "DefaultUserImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        sc.DefaultUserPicture.SaveAs(finalpath);

                        obj8.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }

                    objAdmin.SystemConfigurations.Add(obj8);
                    objAdmin.SaveChanges();

                    TempData["AdminDashboard"] = userObj.FirstName + " " + userObj.LastName;
                    TempData["Message"] = "System Configuration has been Successfully Added !";
                }
                UserProfile userprofile = objAdmin.UserProfile.Where(x => x.UserID == userObj.UserID).FirstOrDefault();
               if(userprofile == null)
                {
                    return RedirectToAction("UpdateProfileAdmin","Admin");
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }

        [Route("Administrators")]
        public ActionResult Administrators(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.FirstNameSortParam = SortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewBag.LastNameSortParam = SortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.EmailIDSortParam = SortOrder == "EmailID" ? "EmailID_desc" : "EmailID";
            ViewBag.ActiveSortParam = SortOrder == "Active" ? "Active_desc" : "Active";

            var admin = objAdmin.Users.Where(x =>x.RoleID == objAdmin.UserRoles.Where(y=>y.Name.ToLower() == "admin").Select(z=>z.RoleID).FirstOrDefault() && (x.FirstName.Contains(Search) || x.LastName.Contains(Search) || x.EmailID.Contains(Search)
            || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search)
            || x.UserProfile.Select(y=>y.PhoneNumberCountryCode).ToList().Contains(Search)||x.UserProfile.All(y=>y.PhoneNumber.Contains(Search))|| x.UserProfile.Select(y => y.PhoneNumber).ToList().Contains(Search) || Search == null)).AsQueryable();
            ViewBag.UsersProfiles = objAdmin.UserProfile.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    admin = admin.OrderBy(x => x.ModifiedDate);
                    break;
                case "FirstName_desc":
                    admin = admin.OrderByDescending(x => x.FirstName);
                    break;
                case "FirstName":
                    admin = admin.OrderBy(x => x.FirstName);
                    break;
                case "LastName_desc":
                    admin = admin.OrderByDescending(x => x.LastName);
                    break;
                case "LastName":
                    admin = admin.OrderBy(x => x.LastName);
                    break;
                case "EmailID_desc":
                    admin = admin.OrderByDescending(x => x.EmailID);
                    break;
                case "EmailID":
                    admin = admin.OrderBy(x => x.EmailID);
                    break;
                case "Active_desc":
                    admin = admin.OrderByDescending(x => x.IsActive);
                    break;
                case "Active":
                    admin = admin.OrderBy(x => x.IsActive);
                    break;
                default:
                    admin = admin.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(admin.ToList().ToPagedList(page ?? 1, 5));
            
        }

        [Route("AddAdministrator")]
        [HttpGet]
        public ActionResult AddAdministrator()
        {
            ViewBag.PhoneNumberCountryCode = objAdmin.Countries.Distinct().Where(x => x.IsActive == true).ToList();
            /*ViewBag.PhoneNumberCountryCode = new SelectList(objAdmin.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode");*/
            return View();
        }

        [Route("AddAdministrator")]
        [HttpPost]
        public ActionResult AddAdministrator(AddAdministrator addadministrator)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                /*ViewBag.PhoneNumberCountryCode = new SelectList(objAdmin.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode");*/
                Users admin = new Users
                {
                    FirstName = addadministrator.FirstName,
                    LastName = addadministrator.LastName,
                    EmailID = addadministrator.EmailID,
                    RoleID = objAdmin.UserRoles.Where(x => x.Name.ToLower() == "admin").Select(y => y.RoleID).FirstOrDefault(),
                    Password = EncryptPassword.EncryptPasswordMd5("Admin@123"),
                    CreatedBy = userObj.UserID,
                    ModifiedBy = userObj.UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now

                };
                objAdmin.Users.Add(admin);

                string activationCode = Guid.NewGuid().ToString();
                admin.VerificationCode = activationCode;
                objAdmin.Users.Add(admin);
                objAdmin.SaveChanges();

                // Generating Email Verification Link
                /*var activationCode = obj.Password;*/
                var verifyUrl = "/Account/VerifyAccount/" + activationCode;
                var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                var emailid = objAdmin.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
                var password = objAdmin.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
                // Sending Email
                EmailVerificationLink.SendVerificationLinkEmail(admin, activationlink,emailid,password);

                var id = admin.UserID;

                UserProfile adminprofile = new UserProfile
                {
                    PhoneNumberCountryCode = addadministrator.PhoneNumberCountryCode,
                    PhoneNumber = addadministrator.PhoneNumber,
                    UserID = id,
                    ProfilePicture = objAdmin.SystemConfigurations.Where(x => x.keys == "DefaultUserPicture").Select(x => x.Value).FirstOrDefault(),
                    CreatedBy = userObj.UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = userObj.UserID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
               
                objAdmin.UserProfile.Add(adminprofile);
                objAdmin.SaveChanges();

                string storepath = Path.Combine(Server.MapPath("~/Members/"), id.ToString());
                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }
                TempData["AddAdministrator"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Administrator has been Successfully Added !";
                return RedirectToAction("Administrators", "Admin");
            }
            return View();
        }

        [Route("EditAdministrator/{id}")]
        [HttpGet]
        public ActionResult EditAdministrator(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users admin = objAdmin.Users.Find(id);
            UserProfile adminprofile = objAdmin.UserProfile.Where(x => x.UserID == admin.UserID).FirstOrDefault();

            EditAdministrator addadministrator = new EditAdministrator
            {
                UserID = admin.UserID,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                EmailID = admin.EmailID,
                PhoneNumber = adminprofile.PhoneNumber,
                IsEmailVerified = admin.IsEmailVerified
            };

            if (admin == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            ViewBag.PhoneNumberCountryCode = new SelectList(objAdmin.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode", adminprofile.PhoneNumberCountryCode);
            return View(addadministrator);
        }

        [Route("EditAdministrator/{id}")]
        [HttpPost]
        public ActionResult EditAdministrator(int? id, EditAdministrator addadministrator)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Users admin = objAdmin.Users.Find(id);
                UserProfile adminprofile = objAdmin.UserProfile.Where(x => x.UserID == admin.UserID).FirstOrDefault();
                if (admin == null)
                {
                    return RedirectToAction("Error", "HomePage");
                }

                var Emailid = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                admin.FirstName = addadministrator.FirstName;
                admin.LastName = addadministrator.LastName;
                admin.EmailID = addadministrator.EmailID;
                admin.ModifiedBy = userObj.UserID;
                admin.ModifiedDate = DateTime.Now;

                objAdmin.Entry(admin).State = EntityState.Modified;
                objAdmin.SaveChanges();

                adminprofile.PhoneNumberCountryCode = addadministrator.PhoneNumberCountryCode;
                adminprofile.PhoneNumber = addadministrator.PhoneNumber;

                objAdmin.Entry(adminprofile).State = EntityState.Modified;
                objAdmin.SaveChanges();

                TempData["AddAdministrator"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Administrator has been Successfully Edited !";
                return RedirectToAction("Administrators", "Admin");
            }
            return View();
        }

        [Route("DeleteAdministrator/{id}")]
        [HttpGet]
        public ActionResult DeleteAdministrator(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users admin = objAdmin.Users.Find(id);

            if (admin == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            admin.IsActive = false;

            objAdmin.Entry(admin).State = EntityState.Modified;
            objAdmin.SaveChanges();

            /* objAdmin.NoteCategories.Remove(notecategory);
             objAdmin.SaveChanges();*/
            var Emailid = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
            TempData["AddAdministrator"] = userObj.FirstName + " " + userObj.LastName;
            TempData["Message"] = "Administrator has been Successfully Deleted !";
            return RedirectToAction("Administrators", "Admin");

        }
    }
}