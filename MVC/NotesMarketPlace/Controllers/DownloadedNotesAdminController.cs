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
    public class DownloadedNotesAdminController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();

        [Route("DownloadNotes")]
        // GET: DownloadedNotesAdmin
        public ActionResult DownloadNotes(int? page,string SellerName, string BuyerName,string Search,string AllNotes,string SortOrder)
        {
            List<Downloads> downloads = objAdmin.Downloads.Where(x => x.IsActive == true && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null && x.IsAttachmentDownloaded == true && (x.NoteTitle.Contains(Search)|| x.NoteCategory.Contains(Search)|| x.PurchasedPrice.ToString().StartsWith(Search)||x.Users.FirstName.Contains(Search)||x.Users.LastName.Contains(Search)
            || (x.AttachmentDownloadedDate.Value.Day + "-" + x.AttachmentDownloadedDate.Value.Month + "-" + x.AttachmentDownloadedDate.Value.Year).Contains(Search)
            || Search == null)).ToList();
            List<Users> users = objAdmin.Users.Where(x => x.RoleID == objAdmin.UserRoles.Where(y => y.Name.ToLower() == "member").Select(y => y.RoleID).FirstOrDefault() && x.IsEmailVerified == true && x.IsActive == true).ToList();

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParam = SortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.BuyerSortParam = SortOrder == "Buyer" ? "Buyer_desc" : "Buyer";
            ViewBag.SellTypeSortParam = SortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParam = SortOrder == "Price" ? "Price_desc" : "Price";
            
            var downloadsnotes = (from nt in downloads
                                  join sell in users on nt.Seller equals sell.UserID into table1
                                  from sell in table1.ToList()
                                  join down in users on nt.Downloader equals down.UserID into table2
                                  from down in table2.ToList()
                                  where (((sell.FirstName + sell.LastName) == SellerName || string.IsNullOrEmpty(SellerName))&&
                                  ((down.FirstName + down.LastName) == BuyerName || string.IsNullOrEmpty(BuyerName)) &&
                                  ((nt.NoteTitle) == AllNotes || string.IsNullOrEmpty(AllNotes))
                                  )
                                  select new AdminDownloadNotes
                                  {
                                      downloads = nt,
                                      seller = sell,
                                      buyer = down,                                 
                                  }).AsQueryable();

            var Seller = objAdmin.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1 && x.IsActive == true)
   .Select(s => new
   {
       Text = s.FirstName + "" + s.LastName,
   }).Distinct().ToList();

            var Notes = objAdmin.Downloads.Where(x => x.IsActive == true)
   .Select(s => new
   {
       Text = s.NoteTitle,
   }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            ViewBag.BuyerName = new SelectList(Seller, "Text", "Text");
            ViewBag.AllNotes = new SelectList(Notes, "Text", "Text");


            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.downloads.AttachmentDownloadedDate);
                    break;
                case "Title_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.downloads.NoteTitle);
                    break;
                case "Title":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.downloads.NoteTitle);
                    break;
                case "Category_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.downloads.NoteCategory);
                    break;
                case "Category":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.downloads.NoteCategory);
                    break;
                case "Seller_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.seller.FirstName);
                    break;
                case "Seller":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.seller.FirstName);
                    break;
                case "Buyer_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.buyer.FirstName);
                    break;
                case "Buyer":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.buyer.FirstName);
                    break;
                case "SellType_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.downloads.IsPaid);
                    break;
                case "SellType":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.downloads.IsPaid);
                    break;
                case "Price_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.downloads.PurchasedPrice);
                    break;
                case "Price":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.downloads.PurchasedPrice);
                    break;
               
                default:
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.downloads.AttachmentDownloadedDate);
                    break;
            }

            ViewBag.downloadsnotes = downloadsnotes.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }
    }
}