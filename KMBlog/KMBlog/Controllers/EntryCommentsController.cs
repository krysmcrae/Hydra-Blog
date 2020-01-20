using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KMBlog.Models;
using Microsoft.AspNet.Identity;

namespace KMBlog.Controllers
{
    public class EntryCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EntryComments
        public ActionResult Index()
        {
            var entryComments = db.EntryComments.Include(e => e.Author).Include(e => e.BlogEntry);
            return View(entryComments.ToList());
        }

        // GET: EntryComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryComment entryComment = db.EntryComments.Find(id);
            if (entryComment == null)
            {
                return HttpNotFound();
            }
            return View(entryComment);
        }

        // GET: EntryComments/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.BlogEntryId = new SelectList(db.Posts, "Id", "Slug");
            return View();
        }

        // POST: EntryComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogEntryId,Body")] EntryComment entryComment)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                entryComment.AuthorId = userId;
                entryComment.CreationDate = DateTime.Now;
                db.EntryComments.Add(entryComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", entryComment.AuthorId);
            ViewBag.BlogEntryId = new SelectList(db.Posts, "Id", "Slug", entryComment.BlogEntryId);
            return View(entryComment);
        }

        // GET: EntryComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryComment entryComment = db.EntryComments.Find(id);
            if (entryComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", entryComment.AuthorId);
            ViewBag.BlogEntryId = new SelectList(db.Posts, "Id", "Slug", entryComment.BlogEntryId);
            return View(entryComment);
        }

        // POST: EntryComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BlogEntryId,AuthorId,Body,CreationDate,UpdatedDate,UpdateReason")] EntryComment entryComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entryComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", entryComment.AuthorId);
            ViewBag.BlogEntryId = new SelectList(db.Posts, "Id", "Slug", entryComment.BlogEntryId);
            return View(entryComment);
        }

        // GET: EntryComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryComment entryComment = db.EntryComments.Find(id);
            if (entryComment == null)
            {
                return HttpNotFound();
            }
            return View(entryComment);
        }

        // POST: EntryComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EntryComment entryComment = db.EntryComments.Find(id);
            db.EntryComments.Remove(entryComment);
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
