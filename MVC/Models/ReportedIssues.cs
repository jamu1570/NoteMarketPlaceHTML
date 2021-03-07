using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class ReportedIssues
    {
        /*[Required(ErrorMessage = "Please Enter Remarks")]*/
        public string Remarks { get; set; }
    }
}