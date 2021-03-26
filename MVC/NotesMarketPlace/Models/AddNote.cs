using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class AddNote
    {
        public int SellerNotesID { get; set; }

        [StringLength(60, MinimumLength = 3, ErrorMessage = "Enter Note Title between 3 to 60")]
        [Required(ErrorMessage = "Please Enter Note Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Select Note Category")]
        public int Category { get; set; }
        /*[FileExtensions("jpg,jpeg,png", ErrorMessage = "Only .jpg, .jpeg, and .png extensions are allowed.")]*/
        /*[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$", ErrorMessage = "Only pdf files allowed.")]*/
        public HttpPostedFileBase DisplayPicture { get; set; }

        [Required(ErrorMessage = "Please Select A file to upload")]
        public List<HttpPostedFileBase> UploadNotes { get; set; }

        /*[Required(ErrorMessage = "Please Select Note Type")]*/
        public Nullable<int> NoteType { get; set; }
        public Nullable<int> NumberOfPages { get; set; }

        [Required(ErrorMessage = "Please Enter Note Description")]
        public string Description { get; set; }

        public string UniversityName { get; set; }

       /* [Required(ErrorMessage = "Please Select Country")]*/
        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }

        [Required(ErrorMessage = "Please Select IsPaid")]
        public bool IsPaid { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public HttpPostedFileBase NotePreview { get; set; }

        public string Display_picture { get; set; }

    }
}