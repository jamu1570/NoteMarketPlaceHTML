using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Models
{
    public class UpdateProfileAdmin
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "only alphabet allowed !")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "only alphabet allowed !")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter EmailID")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please Enter SecondaryEmailID")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [Remote("EmailExitsAdmin", "Account", ErrorMessage = "Email is Already Exist")]
        public string SecondaryEmailAddress { get; set; }

        [Required(ErrorMessage = "Please select Country Code")]
        public string PhoneNumberCountryCode { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "Only Numerics Allowed with max 12 !")]
        public string PhoneNumber { get; set; }

        public HttpPostedFileBase ProfilePicture { get; set; }
    }
}