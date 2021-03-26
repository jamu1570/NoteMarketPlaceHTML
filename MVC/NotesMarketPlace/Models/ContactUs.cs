using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Please Enter Your Full Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "only alphabet allowed !")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter EmailID")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        /*[Remote("EmailExits", "Account", ErrorMessage = "You Entered Registered Email ! Please Login !")]*/
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please Enter Subject")]
        /*[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "only alphabet allowed !")]*/
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter Questions/Comments")]
        public string Comments { get; set; }
    }
}