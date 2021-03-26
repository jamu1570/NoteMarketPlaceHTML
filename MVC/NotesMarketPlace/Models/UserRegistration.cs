using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only alphabet allowed !")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only alphabet allowed !")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter EmailID")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [Remote("EmailExits", "Account", ErrorMessage = "Email is Already Exist")]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!*()@%&]).{6,24}$", ErrorMessage = "Invalid password format")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please Enter  Confirm Password")]
        /*[RegularExpression("^(?=.[a-z])(?=.[A-Z])(?=.[0-9])(?=.[#$^+=!*()@%&]).{6,24}$", ErrorMessage = "Invalid password format")]*/
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password And Confirm Password Does not match")]
        public string ConfirmPassword { get; set; }
    }
}