using NotesMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class NoteCategoriesController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();
        // GET: NoteCategories
        [Route("Categories")]
        public ActionResult Categories(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.NameSortParam = SortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.DescriptionSortParam = SortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.AddedBySortParam = SortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";

            var category = objAdmin.NoteCategories.Where(x =>x.Name.Contains(Search) || x.Description.Contains(Search)
             || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search)
            || objAdmin.Users.Select(y=>y.FirstName).ToList().Contains(Search) || Search == null).AsQueryable();
            ViewBag.Users = objAdmin.Users.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    category = category.OrderBy(x => x.ModifiedDate);
                    break;
                case "Name_desc":
                    category = category.OrderByDescending(x => x.Name);
                    break;
                case "Name":
                    category = category.OrderBy(x => x.Name);
                    break;
                case "Description_desc":
                    category = category.OrderByDescending(x => x.Description);
                    break;
                case "Description":
                    category = category.OrderBy(x => x.Description);
                    break;
                case "AddedBy_desc":
                    category = category.OrderByDescending(x => x.ModifiedBy);
                    break;
                case "AddedBy":
                    category = category.OrderBy(x => x.ModifiedBy);
                    break;
                default:
                    category = category.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(category.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("AddCategory")]
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [Route("AddCategory")]
        [HttpPost]
        public ActionResult AddCategory(AddCategory addcategory)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                NoteCategories notecategory = new NoteCategories()
                {
                    Name = addcategory.Name,
                    Description = addcategory.Description,
                    CreatedBy = userObj.UserID,
                    ModifiedBy = userObj.UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                objAdmin.NoteCategories.Add(notecategory);
                objAdmin.SaveChanges();

                TempData["Category"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Category has been Successfully Added !";
                return RedirectToAction("Categories", "Admin");
            }
            return View();
        }

        [Route("EditCategory/{id}")]
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NoteCategories notecategory = objAdmin.NoteCategories.Find(id);

            if (notecategory == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            AddCategory addcategory = new AddCategory
            {
                Name = notecategory.Name,
                Description = notecategory.Description,
                CategoryID = notecategory.CategoryID
            };

            return View(addcategory);
        }

        [Route("EditCategory/{id}")]
        [HttpPost]
        public ActionResult EditCategory(int? id, AddCategory addcategory)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                NoteCategories notecategory = objAdmin.NoteCategories.Where(x => x.CategoryID == addcategory.CategoryID).FirstOrDefault();
                notecategory.Name = addcategory.Name;
                notecategory.Description = addcategory.Description;
                notecategory.ModifiedBy = userObj.UserID;
                notecategory.ModifiedDate = DateTime.Now;

                objAdmin.Entry(notecategory).State = EntityState.Modified;
                objAdmin.SaveChanges();
                TempData["Category"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Category has been Successfully Edited !";
                
                return RedirectToAction("Categories", "Admin");
            }
            return View();
        }

        [Route("DeleteCategory/{id}")]
        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteCategories notecategory = objAdmin.NoteCategories.Find(id);

            if (notecategory == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            notecategory.IsActive = false;
            objAdmin.Entry(notecategory).State = EntityState.Modified;
            objAdmin.SaveChanges();

            /*objAdmin.NoteCategories.Remove(notecategory);
            objAdmin.SaveChanges();*/
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            TempData["Category"] = userObj.FirstName + " " + userObj.LastName;
            TempData["Message"] = "Category has been Successfully Deleted !";
         
            return RedirectToAction("Categories", "Admin");

        }
    }
}