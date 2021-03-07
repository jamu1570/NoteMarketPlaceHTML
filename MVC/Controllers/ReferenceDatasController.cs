 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Models;

namespace NotesMarketPlace.Controllers
{
    public class ReferenceDatasController : Controller
    {
        private NotesMarketPlaceEntities db = new NotesMarketPlaceEntities();

        // GET: ReferenceDatas
        public async Task<ActionResult> Index()
        {
            return View(await db.ReferenceData.ToListAsync());
        }

        // GET: ReferenceDatas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReferenceData referenceData = await db.ReferenceData.FindAsync(id);
            if (referenceData == null)
            {
                return HttpNotFound();
            }
            return View(referenceData);
        }

        // GET: ReferenceDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReferenceDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReferenceID,Value,DataValue,RefCategory,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] ReferenceData referenceData)
        {
            if (ModelState.IsValid)
            {
                db.ReferenceData.Add(referenceData);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(referenceData);
        }

        // GET: ReferenceDatas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReferenceData referenceData = await db.ReferenceData.FindAsync(id);
            if (referenceData == null)
            {
                return HttpNotFound();
            }
            return View(referenceData);
        }

        // POST: ReferenceDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReferenceID,Value,DataValue,RefCategory,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] ReferenceData referenceData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referenceData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(referenceData);
        }

        // GET: ReferenceDatas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReferenceData referenceData = await db.ReferenceData.FindAsync(id);
            if (referenceData == null)
            {
                return HttpNotFound();
            }
            return View(referenceData);
        }

        // POST: ReferenceDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReferenceData referenceData = await db.ReferenceData.FindAsync(id);
            db.ReferenceData.Remove(referenceData);
            await db.SaveChangesAsync();
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
