using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class SystemConfigurationSuperAdmin
    {
        [Required(ErrorMessage = "Please Enter EmailID")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string SupportEmailID { get; set; }

        [Required(ErrorMessage = "Please Enter EmailID Passowrd")]
        public string SupportEmailIDPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "Only Numerics Allowed with max 12 !")]
        public string SupportPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter EmailID")]
        /*[RegularExpression("(([a-zA-Z/-0-9/.]+@)([a-zA-Z/-0-9/.]+)[,]*)+", ErrorMessage = "Enter Valid Emails !")]*/
        /*([A - Za - z0 - 9._ % +-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}(;|$))*/
        /*[EmailAddress(ErrorMessage = "Enter valid email address")] */   
        public string EmailIDs { get; set; }

        [Required(ErrorMessage = "Please Enter FBURL")]
        [Url(ErrorMessage = "Invalid URL!")]
        public string FBURL { get; set; }
        [Required(ErrorMessage = "Please Enter LinkedInURL")]
        [Url(ErrorMessage = "Invalid URL!")]
        public string LinkedInURL { get; set; }
        [Required(ErrorMessage = "Please Enter TwitterURL")]
        [Url(ErrorMessage = "Invalid URL!")]
        public string TwitterURL { get; set; }
        [Required(ErrorMessage = "Please Enter DefaultNotePicture")]
        public HttpPostedFileBase DefaultNotePicture { get; set; }
        [Required(ErrorMessage = "Please Enter DefaultUserPicture")]
        public HttpPostedFileBase DefaultUserPicture { get; set; }
    }
}