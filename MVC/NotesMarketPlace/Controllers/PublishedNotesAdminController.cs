using NotesMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class PublishedNotesAdminController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();
        // GET: PublishedNotesAdmin
        [Route("PublishedNotes")]
        public ActionResult PublishedNotes(string Search, int? page, string SortOrder, string SellerName)
        {
            List<SellerNotes> NoteTitlePublished = objAdmin.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(Search)||x.NoteCategories.Name.Contains(Search) ||x.SellingPrice.ToString().StartsWith(Search)||x.Users.FirstName.Contains(Search)||x.Users.LastName.Contains(Search)
            || (x.PublishedDate.Value.Day + "-" + x.PublishedDate.Value.Month + "-" + x.PublishedDate.Value.Year).Contains(Search)
            || Search == null)).ToList();

            List<NoteCategories> CategoryNamePublished = objAdmin.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = objAdmin.ReferenceData.Where(x => x.RefCategory == "Note Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();
            List<Users> UserDetails = objAdmin.Users.ToList();

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParam = SortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.SellTypeSortParam = SortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParam = SortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.ApprovedBySortParam = SortOrder == "ApprovedBy" ? "ApprovedBy_desc" : "ApprovedBy";

            var NotesPublished = (from nt in NoteTitlePublished
                                    join cn in CategoryNamePublished on nt.Category equals cn.CategoryID into table1
                                    from cn in table1.ToList()
                                    join sn in StatusNamePublished on nt.Status equals sn.ReferenceID into table2
                                    from sn in table2.ToList()
                                    join us in UserDetails on nt.SellerID equals us.UserID into table3
                                    from us in table3.ToList()
                                    join ad in UserDetails on nt.ActionedBy equals ad.UserID into table4
                                    from ad in table4.ToList()
                                  where ((sn.Value == "Published") && ((us.FirstName + us.LastName) == SellerName || string.IsNullOrEmpty(SellerName)))
                                    select new PublishedNoteAdmin
                                    {
                                        NoteDetails = nt,
                                        Category = cn,
                                        status = sn,
                                        user = us,
                                        admin =ad
                                    }).AsQueryable();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.PublishedDate);
                    break;
                case "Title_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    NotesPublished = NotesPublished.OrderBy(x => x.Category.Name);
                    break;
                case "Seller_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.user.FirstName);
                    break;
                case "Seller":
                    NotesPublished = NotesPublished.OrderBy(x => x.user.FirstName);
                    break;
                case "SellType_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.IsPaid);
                    break;
                case "SellType":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.IsPaid);
                    break;
                case "Price_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.SellingPrice);
                    break;
                case "Price":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.SellingPrice);
                    break;
                case "ApprovedBy_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.user.FirstName);
                    break;
                case "ApprovedBy":
                    NotesPublished = NotesPublished.OrderBy(x => x.user.FirstName);
                    break;
                default:
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.PublishedDate);
                    break;
            }

            var Seller = objAdmin.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1 && x.IsActive == true)
    .Select(s => new
    {
        Text = s.FirstName + "" + s.LastName,
    }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            /*ViewBag.SellerName = objAdmin.Users.Select(x => new { x.FirstName }).Distinct().Where(x => x.FirstName != null).ToList();*/
            /* ViewBag.SellerName = objAdmin.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1).Select(x =>x.FirstName + x.LastName).Distinct().ToList();*/
            ViewBag.NotesPublished = NotesPublished.ToList().ToPagedList(page ?? 1, 4);
            return View();
        }
    }
}