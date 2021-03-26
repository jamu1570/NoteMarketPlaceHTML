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
    public class NoteTypesController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();
        // GET: NoteTypes
        [Route("Types")]
        public ActionResult Types(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.NameSortParam = SortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.DescriptionSortParam = SortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.AddedBySortParam = SortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";

            var type = objAdmin.NoteTypes.Where(x =>x.Name.Contains(Search) || x.Description.Contains(Search)
            || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search)
            || Search == null).AsQueryable();
            ViewBag.Users = objAdmin.Users.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    type = type.OrderBy(x => x.ModifiedDate);
                    break;
                case "Name_desc":
                    type = type.OrderByDescending(x => x.Name);
                    break;
                case "Name":
                    type = type.OrderBy(x => x.Name);
                    break;
                case "Description_desc":
                    type = type.OrderByDescending(x => x.Description);
                    break;
                case "Description":
                    type = type.OrderBy(x => x.Description);
                    break;
                case "AddedBy_desc":
                    type = type.OrderByDescending(x => x.ModifiedBy);
                    break;
                case "AddedBy":
                    type = type.OrderBy(x => x.ModifiedBy);
                    break;
                default:
                    type = type.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(type.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("AddType")]
        [HttpGet]
        public ActionResult AddType()
        {
            return View();
        }

        [Route("AddType")]
        [HttpPost]
        public ActionResult AddType(AddType addtype)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                NoteTypes notetype = new NoteTypes()
                {
                    Name = addtype.Name,
                    Description = addtype.Description,
                    CreatedBy = userObj.UserID,
                    ModifiedBy = userObj.UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                objAdmin.NoteTypes.Add(notetype);
                objAdmin.SaveChanges();
                TempData["Type"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Category has been Successfully Added !";
                return RedirectToAction("Types", "Admin");
            }
            return View();
        }

        [Route("EditType/{id}")]
        [HttpGet]
        public ActionResult EditType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NoteTypes notetype = objAdmin.NoteTypes.Find(id);

            if (notetype == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            AddType addtype = new AddType
            {
                Name = notetype.Name,
                Description = notetype.Description,
                TypeID = notetype.TypeID
            };

            return View(addtype);
        }

        [Route("EditType/{id}")]
        [HttpPost]
        public ActionResult EditType(int? id, AddType addtype)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                NoteTypes notetype = objAdmin.NoteTypes.Where(x => x.TypeID == addtype.TypeID).FirstOrDefault();
                notetype.Name = addtype.Name;
                notetype.Description = addtype.Description;
                notetype.ModifiedBy = userObj.UserID;
                notetype.ModifiedDate = DateTime.Now;

                objAdmin.Entry(notetype).State = EntityState.Modified;
                objAdmin.SaveChanges();
                TempData["Type"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Category has been Successfully Edited !";
                return RedirectToAction("Types", "Admin");
            }
            return View();
        }

        [Route("DeleteType/{id}")]
        [HttpGet]
        public ActionResult DeleteType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteTypes notetype = objAdmin.NoteTypes.Find(id);

            if (notetype == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            notetype.IsActive = false;
            objAdmin.Entry(notetype).State = EntityState.Modified;
            objAdmin.SaveChanges();
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            TempData["Type"] = userObj.FirstName + " " + userObj.LastName;
            TempData["Message"] = "Category has been Successfully Deleted !";
            return RedirectToAction("Types", "Admin");

        }

    }
}