using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Login
    {

        [Required(ErrorMessage = "Please Enter EmailID")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}