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
    public class BlogEntriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogEntries
        public ActionResult Index()
        {
            var publishedEntries = db.Posts.Where(p => p.Published).ToList();
            return View(publishedEntries);
        }

        // GET: BlogEntries/Details/5
        public ActionResult Details(string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogEntry blogEntry = db.Posts.FirstOrDefault(p => p.Slug == Slug);
            if (blogEntry == null)
            {
                return HttpNotFound();
            }
            return View(blogEntry);
        }

        // GET: BlogEntries/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: BlogEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,AuthorId,Abstract,Body,MediaURL,Published")] BlogEntry blogEntry)
        {
            if (ModelState.IsValid)
            {
                var Slug = StringUtilities.URLFriendly(blogEntry.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title");
                    return View(blogEntry);
                }
                if (db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogEntry);
                }
                blogEntry.Slug = Slug;
                var userId = User.Identity.GetUserId();
                blogEntry.AuthorId = userId;
                blogEntry.CreationDate = DateTime.Now;
                db.Posts.Add(blogEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogEntry);
        }

        // GET: BlogEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogEntry blogEntry = db.Posts.Find(id);
            if (blogEntry == null)
            {
                return HttpNotFound();
            }
            return View(blogEntry);
        }

        // POST: BlogEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,AuthorId,Slug,Abstract,Body,CreationDate,UpdatedDate,MediaURL,Published")] BlogEntry blogEntry)
        {
            if (ModelState.IsValid)
            {
                var oldPost = db.Posts.AsNoTracking().FirstOrDefault(p => p.Id == blogEntry.Id);
                blogEntry.UpdatedDate = DateTime.Now;
                db.Entry(blogEntry).State = EntityState.Modified;
                db.SaveChanges();

                var newPost = db.Posts.AsNoTracking().FirstOrDefault(p => p.Id == blogEntry.Id);
                if(oldPost.Title != newPost.Title)
                {
                    var Slug = StringUtilities.URLFriendly(blogEntry.Title);
                    if (String.IsNullOrWhiteSpace(Slug))
                    {
                        ModelState.AddModelError("Title", "Invalid Title");
                        return View(blogEntry);
                    }
                    if (db.Posts.Any(p => p.Slug == Slug))
                    {
                        ModelState.AddModelError("Title", "The title must be unique");
                        return View(blogEntry);
                    }
                    blogEntry.Slug = Slug;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(blogEntry);
        }

        // GET: BlogEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogEntry blogEntry = db.Posts.Find(id);
            if (blogEntry == null)
            {
                return HttpNotFound();
            }
            return View(blogEntry);
        }

        // POST: BlogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogEntry blogEntry = db.Posts.Find(id);
            db.Posts.Remove(blogEntry);
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
