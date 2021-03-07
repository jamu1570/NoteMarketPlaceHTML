using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Models;

namespace NotesMarketPlace.Controllers
{
    public class NoteCategoriesController : Controller
    {
        private NotesMarketPlaceEntities db = new NotesMarketPlaceEntities();

        // GET: NoteCategories
        public ActionResult Index()
        {
            return View(db.NoteCategories.ToList());
        }

        // GET: NoteCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteCategories noteCategories = db.NoteCategories.Find(id);
            if (noteCategories == null)
            {
                return HttpNotFound();
            }
            return View(noteCategories);
        }

        // GET: NoteCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoteCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,Name,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] NoteCategories noteCategories)
        {
            if (ModelState.IsValid)
            {
                db.NoteCategories.Add(noteCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noteCategories);
        }

        // GET: NoteCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteCategories noteCategories = db.NoteCategories.Find(id);
            if (noteCategories == null)
            {
                return HttpNotFound();
            }
            return View(noteCategories);
        }

        // POST: NoteCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,Name,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] NoteCategories noteCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noteCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noteCategories);
        }

        // GET: NoteCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteCategories noteCategories = db.NoteCategories.Find(id);
            if (noteCategories == null)
            {
                return HttpNotFound();
            }
            return View(noteCategories);
        }

        // POST: NoteCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NoteCategories noteCategories = db.NoteCategories.Find(id);
            db.NoteCategories.Remove(noteCategories);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
