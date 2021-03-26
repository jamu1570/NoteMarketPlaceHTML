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
    public class NoteCountriesController : Controller
    {
        private NotesMarketPlaceEntities objAdmin = new NotesMarketPlaceEntities();
        // GET: NoteCountries
        [Route("Countries")]
        public ActionResult Countries(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.NameSortParam = SortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.CountryCodeSortParam = SortOrder == "CountryCode" ? "CountryCode_desc" : "CountryCode";
            ViewBag.AddedBySortParam = SortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";

            var country = objAdmin.Countries.Where(x => x.Name.Contains(Search) || x.CountryCode.Contains(Search)
             || (x.ModofiedDate.Value.Day + "-" + x.ModofiedDate.Value.Month + "-" + x.ModofiedDate.Value.Year).Contains(Search)
            || Search == null).AsQueryable();
            ViewBag.Users = objAdmin.Users.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    country = country.OrderBy(x => x.ModofiedDate);
                    break;
                case "Name_desc":
                    country = country.OrderByDescending(x => x.Name);
                    break;
                case "Name":
                    country = country.OrderBy(x => x.Name);
                    break;
                case "CountryCode_desc":
                    country = country.OrderByDescending(x => x.CountryCode);
                    break;
                case "CountryCode":
                    country = country.OrderBy(x => x.CountryCode);
                    break;
                case "AddedBy_desc":
                    country = country.OrderByDescending(x => x.ModifiedBy);
                    break;
                case "AddedBy":
                    country = country.OrderBy(x => x.ModifiedBy);
                    break;
                default:
                    country = country.OrderByDescending(x => x.ModofiedDate);
                    break;
            }

            return View(country.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("Addcountry")]
        [HttpGet]
        public ActionResult Addcountry()
        {
            return View();
        }

        [Route("Addcountry")]
        [HttpPost]
        public ActionResult Addcountry(AddCountry addcountry)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                Countries notecountry = new Countries()
                {
                    Name = addcountry.Name,
                    CountryCode = addcountry.CountryCode,
                    CreatedBy = userObj.UserID,
                    ModifiedBy = userObj.UserID,
                    CreatedDate = DateTime.Now,
                    ModofiedDate = DateTime.Now,
                    IsActive = true
                };
                objAdmin.Countries.Add(notecountry);
                objAdmin.SaveChanges();
                TempData["Country"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Category has been Successfully Added !";
                return RedirectToAction("Countries", "Admin");
            }
            return View();
        }

        [Route("EditCountry/{id}")]
        [HttpGet]
        public ActionResult EditCountry(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Countries notecountry = objAdmin.Countries.Find(id);

            if (notecountry == null)
            {
                return RedirectToAction("Error", "HomePage");
            }

            AddCountry addcountry = new AddCountry
            {
                Name = notecountry.Name,
                CountryCode = notecountry.CountryCode,
                CoutryID = notecountry.CoutryID
            };

            return View(addcountry);
        }

        [Route("EditCountry/{id}")]
        [HttpPost]
        public ActionResult EditCountry(int? id, AddCountry addcountry)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                Countries notecountry = objAdmin.Countries.Where(x => x.CoutryID == addcountry.CoutryID).FirstOrDefault();
                notecountry.Name = addcountry.Name;
                notecountry.CountryCode = addcountry.CountryCode;
                notecountry.ModifiedBy = userObj.UserID;
                notecountry.ModofiedDate = DateTime.Now;

                objAdmin.Entry(notecountry).State = EntityState.Modified;
                objAdmin.SaveChanges();
                TempData["Country"] = userObj.FirstName + " " + userObj.LastName;
                TempData["Message"] = "Category has been Successfully Edited !";
                return RedirectToAction("Countries", "Admin");
            }
            return View();
        }

        [Route("DeleteCountry/{id}")]
        [HttpGet]
        public ActionResult DeleteCountry(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Countries notecountry = objAdmin.Countries.Find(id);

            if (notecountry == null)
            {
                return RedirectToAction("Error", "HomePage");
            }
            notecountry.IsActive = false;
            objAdmin.Entry(notecountry).State = EntityState.Modified;
            objAdmin.SaveChanges();
            var EmailID = User.Identity.Name.ToString();
            Users userObj = objAdmin.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            TempData["Country"] = userObj.FirstName + " " + userObj.LastName;
            TempData["Message"] = "Category has been Successfully Deleted !";
            return RedirectToAction("Countries", "Admin");

        }
    }
}