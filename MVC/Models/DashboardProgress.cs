using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class InProgressNote
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }
    }

    public class PublishedNote
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }
    }

    public class AllSearchNotes
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }

        public Countries Country { get; set; }

        public SellerNotesReviews Reviews { get; set; }

        public SellerNotesReportedIssues Reports { get; set; }
    }


    public class BuyerRequest
    {
        public Downloads DownloadNote { get; set; }

        public Users BuyerDetail { get; set; }

        public UserProfile UserProfileNote { get; set; }
    }

    public class MyDownloadNote
    {
        public Downloads DownloadNote { get; set; }

        public Users BuyerDetail { get; set; }
      
    }

    public class MySoldNote
    {
        public Downloads DownloadNote { get; set; }

        public Users BuyerDetail { get; set; }

    }
}