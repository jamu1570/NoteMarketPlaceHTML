using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using NotesMarketPlace.Healpers;
using NotesMarketPlace.EmailTemplates;
using System.Web.Security;

namespace NotesMarketPlace.Controllers
{
    [RoutePrefix("Account")]
    public class  AccountController : Controller
    {
        private NotesMarketPlaceEntities objNotesMarketPlaceEntities = new NotesMarketPlaceEntities();
        [Route("Login")]
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Title = "NotesMarketPlace";
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(Login ObjLogin)
        {
            if (ModelState.IsValid)
            {
      
                //Encrypt Password and Save
                var newPassword = EncryptPassword.EncryptPasswordMd5(ObjLogin.Password);

                bool isValid = objNotesMarketPlaceEntities.Users.Any(x => x.EmailID == ObjLogin.EmailID && x.Password == newPassword && x.IsActive == true);
                if (isValid)
                {
                    Users userDetails = objNotesMarketPlaceEntities.Users.Where(x => x.EmailID == ObjLogin.EmailID && x.Password == newPassword && x.IsActive == true).FirstOrDefault();
                    if (userDetails.IsEmailVerified)
                    {
/*                        Response.Cookies["UserID"].Value = userDetails.UserID.ToString();*/
                        FormsAuthentication.SetAuthCookie(ObjLogin.EmailID, ObjLogin.RememberMe);

                        if(userDetails.RoleID == objNotesMarketPlaceEntities.UserRoles.Where(x =>x.Name.ToLower() == "admin" && x.IsActive == true).Select(x=>x.RoleID).FirstOrDefault())
                        {
                            UserProfile userprofile = objNotesMarketPlaceEntities.UserProfile.Where(x => x.UserID == userDetails.UserID).FirstOrDefault();
                            if (userprofile != null)
                            {
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            return RedirectToAction("UpdateProfileAdmin", "Admin");      
                        }
                        else if (userDetails.RoleID == objNotesMarketPlaceEntities.UserRoles.Where(x => x.Name.ToLower() == "superadmin" && x.IsActive == true).Select(x => x.RoleID).FirstOrDefault())
                        {
                            UserProfile userprofile = objNotesMarketPlaceEntities.UserProfile.Where(x => x.UserID == userDetails.UserID).FirstOrDefault();
                            var Records = objNotesMarketPlaceEntities.SystemConfigurations.Count();          
                            if (Records == 0 )
                            {
                                return RedirectToAction("SystemConfiguration", "Admin");
                            }
                            else if (userprofile != null)
                            {
              
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            return RedirectToAction("UpdateProfileAdmin", "Admin");
                        }
                        else
                        {
                            UserProfile userprofile = objNotesMarketPlaceEntities.UserProfile.Where(x => x.UserID == userDetails.UserID ).FirstOrDefault();
                            if (userprofile != null)
                            {
                                return RedirectToAction("SearchNotesPage", "HomePage");
                            }
                            return RedirectToAction("UpdateProfile", "Member");
                        }

                 
                    }
                    TempData["Error"] = "Email Address Is Not Verified";
                    return View();
                }
                TempData["Error"] = "Invalid username or password";
                return View();
            }

            return View();
        }

        [Route("Register")]
        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Title = "NotesMarketPlace";
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(UserRegistration objUser)
        {
            if (ModelState.IsValid)
            {
                
                Users obj = new Users
                {
                    FirstName = objUser.FirstName,
                    LastName = objUser.LastName,
                    EmailID = objUser.EmailID,
                    RoleID = objNotesMarketPlaceEntities.UserRoles.Where(x=>x.Name.ToLower() == "member").Select(x=>x.RoleID).FirstOrDefault(),
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    IsEmailVerified = false,

                };
                string activationCode = Guid.NewGuid().ToString();
                obj.Password = EncryptPassword.EncryptPasswordMd5(objUser.Password);
                obj.VerificationCode = activationCode;
                /*string activationCode = Guid.NewGuid().ToString();*/
                objNotesMarketPlaceEntities.Users.Add(obj);
                objNotesMarketPlaceEntities.SaveChanges();

                // Generating Email Verification Link
                /*var activationCode = obj.Password;*/
                var verifyUrl = "/Account/VerifyAccount/" + activationCode;
                var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                var emailid = objNotesMarketPlaceEntities.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
                var password = objNotesMarketPlaceEntities.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
                // Sending Email
                EmailVerificationLink.SendVerificationLinkEmail(obj, activationlink,emailid,password);
                ViewBag.Title = "Notes_MarketPlace";
                @TempData["UserName"] = obj.FirstName.ToString();
                return new RedirectResult(@"~\Account\EmailVerification\");

            }
            
            return View();

        }

        [Route("VerifyAccount/{VerificationCode}")]
        [HttpGet]
        public ActionResult VerifyAccount(string VerificationCode)
        {
            using (NotesMarketPlaceEntities DBObject = new NotesMarketPlaceEntities())
            {
                DBObject.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
     
                var v = DBObject.Users.Where(x => x.VerificationCode == VerificationCode).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    v.IsActive = true;
                    DBObject.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }

            TempData["Message"] = "Your Email Is Verified You Can Login Here";
            return RedirectToAction("Login", "Account");

        }
        public JsonResult EmailExits(string EmailID)
        {
           // @*!objNoteMarketplaceEntities.Users.Any(u => u.EmailID == EmailID),*@
            return Json(!objNotesMarketPlaceEntities.Users.Any(u => u.EmailID == EmailID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmailExitsAdmin(string SecondaryEmailAddress)
        {
            // @*!objNoteMarketplaceEntities.Users.Any(u => u.EmailID == EmailID),*@
            return Json(!objNotesMarketPlaceEntities.Users.Any(u => u.EmailID == SecondaryEmailAddress), JsonRequestBehavior.AllowGet);
        }

        [Route("EmailVerification")]
        public ActionResult EmailVerification()
        {
            return View();
        }


        [Route("ForgetPassword")]
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [Route("ForgetPassword")]
        [HttpPost]
        public ActionResult ForgetPassword(ForgetPassword obj)
        {
            if (ModelState.IsValid)
            {
                bool isValid = objNotesMarketPlaceEntities.Users.Any(x => x.EmailID == obj.EmailID);
                if (isValid)
                {
                    Users userDetails = objNotesMarketPlaceEntities.Users.Where(x => x.EmailID == obj.EmailID).FirstOrDefault();
                    Random rand = new Random();
                    var otp = rand.Next(10000,99999);
                    var strotp = otp.ToString();

                    //Encrypt Password and Save
                    userDetails.Password = EncryptPassword.EncryptPasswordMd5(strotp);
                    objNotesMarketPlaceEntities.SaveChanges();
                    var emailid = objNotesMarketPlaceEntities.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailid").Select(x => x.Value).FirstOrDefault();
                    var password = objNotesMarketPlaceEntities.SystemConfigurations.Where(x => x.keys.ToLower() == "supportemailidpassword").Select(x => x.Value).FirstOrDefault();
                    //Sent Otp On email address
                    ForgetPasswordEmail.SendOtpToEmail(userDetails, otp,emailid,password);

                    TempData["Message"] = "Otp Sent To Your Registered EmailAddress use it for login";
                    return RedirectToAction("Login", "Account");

                }
                TempData["Error"] = "Invalid EmailAddress";
                return View();
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","HomePage");
        }

    }
}