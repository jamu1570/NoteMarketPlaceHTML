using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
 
        public class AllPublishedNote
        {
            public SellerNotes NoteDetails { get; set; }

            public NoteCategories Category { get; set; }

            public ReferenceData status { get; set; }

            public Users user { get; set; }

            public SellerNotesAttachments attachment { get; set; }
        }

    public class UnderReviewsNote
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }
        public Users user { get; set; }

    }

    public class RejectedNoteAdmin
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }
        public Users user { get; set; }
        public Users admin { get; set; }

    }
    public class SpamReportedAdmin
    {
        public SellerNotesReportedIssues Reports { get; set; }
        public SellerNotes NoteDetails { get; set; }
        public NoteCategories Category { get; set; }
        public Users user { get; set; }
    }

    public class PublishedNoteAdmin
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }
        public Users user { get; set; }

        public Users admin { get; set; }

    }

    public class AdminDownloadNotes
    {
        public Downloads downloads { get; set; }
        public Users seller { get; set; }
        public Users buyer { get; set; }

    }
    public class RejectedNote
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }

    }

    public class MemberNotes
    {
        public SellerNotes NoteDetails { get; set; }

        public NoteCategories Category { get; set; }

        public ReferenceData status { get; set; }

        public Downloads Download { get; set; }
    }

    public class MemberList
    {
        public Users user { get; set; }

        public SellerNotes NoteDetails { get; set; }

        public ReferenceData reference { get; set; }
    }

}