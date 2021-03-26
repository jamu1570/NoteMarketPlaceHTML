using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class UpdateProfile
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

        public Nullable<System.DateTime> DOB { get; set; }

        public Nullable<int> Gender { get; set; }

        [Required(ErrorMessage = "Please select Country Code")]
        public string PhoneNumberCountryCode { get; set; }

        /* public string SecondaryEmailAddress { get; set; }*/

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "Only Numerics Allowed with max 12 !")]
        public string PhoneNumber { get; set; }

        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required(ErrorMessage = "Please Enter AddressLine1")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Please Enter AddressLine2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please Enter Phone City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter Zip Code")]
        [RegularExpression("^[0-9]{1,8}$", ErrorMessage = "Only Numerics Allowed with max 8 !")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        public string Country { get; set; }
        public string University { get; set; }
        public string College { get; set; }
       
        
    }
}