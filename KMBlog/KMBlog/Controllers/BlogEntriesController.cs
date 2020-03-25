using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KMBlog.Enum;
using KMBlog.Models;
using Microsoft.AspNet.Identity;

namespace KMBlog.Controllers
{
    public class BlogEntriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogEntries
        public ActionResult Index(PostType PostType)
        {
            ViewBag.topics = PostType.topics;
            ViewBag.Author = db.Users.Where(u => u.Id == db.Posts.FirstOrDefault().AuthorId).FirstOrDefault().DisplayName;
            if (PostType==PostType.date)
            {
                //ViewBag.Date = PostType.Remove(0, 4).Split({20, 21});
                ViewBag.PostType = PostType;
                ViewBag.topics = PostType.topics;
                var indexedPosts = db.Posts.Where(p => p.CreationDate.Year==2020).ToList();
                return View(indexedPosts);
            }
            if (PostType == PostType.topics)
            {
                var indexedPosts =db.Posts.Where(p => p.Published == true);
                ViewBag.Topic0 = PostType.MVC;
                ViewBag.Topic1 = PostType.Photography;
                ViewBag.Topic2 = PostType.TED_Talks;
                ViewBag.Topic3 =PostType.Brain_Teasers;
                ViewBag.PostType = PostType.topics;
                ViewBag.topics = PostType.topics;

                return View(indexedPosts);
            }
            else
            {
                var indexedPosts = db.Posts.Where(p => p.PostType == PostType).ToList().Where(p=>p.Published==true);
                ViewBag.Topic = PostType;
                ViewBag.PostType = PostType;
                return View(indexedPosts);
            }

           
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
            ViewBag.Author = db.Users.Where(u => u.Id == blogEntry.AuthorId).FirstOrDefault().DisplayName;
            ViewBag.CommentCount = blogEntry.Comments.Count();
            return View(blogEntry);
        }

        // GET: BlogEntries/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            
                //{ "MVC", "Photography", "TED Talks", "Brain Teasers" };

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: BlogEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Abstract,AbstractImageURL,PostType,Body1,Body2,Body3,MediaURL,Published")] BlogEntry blogEntry)
        {
            if (ModelState.IsValid)
            {
                blogEntry.Slug = StringUtilities.URLFriendly(blogEntry.Title);
                if (String.IsNullOrWhiteSpace(blogEntry.Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title");
                    return View(blogEntry);
                }
                if (db.Posts.Any(p => p.Slug == blogEntry.Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogEntry);
                }
                blogEntry.AbstractImageURL = "/Images/" + blogEntry.AbstractImageURL;
                var userId = User.Identity.GetUserId();
                blogEntry.AuthorId = userId;
                blogEntry.CreationDate = DateTime.Now;
                

                db.Posts.Add(blogEntry);
                db.SaveChanges();
                return RedirectToAction("Index",new { PostType = PostType.topics });
            }

            return View(blogEntry);
        }

        // GET: BlogEntries/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string Slug)
        {
            ViewBag.AuthorId = db.Users.Where(u => u.Id == db.Posts.FirstOrDefault().AuthorId).FirstOrDefault().DisplayName;
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

        // POST: BlogEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit([Bind(Include = "Id,Title,AuthorId,Slug,Abstract,Body,CreationDate,UpdatedDate,MediaURL,Published")] BlogEntry blogEntry)
        {
            if (ModelState.IsValid)
            {
                var oldPost = db.Posts.AsNoTracking().FirstOrDefault(p => p.Id == blogEntry.Id);
                blogEntry.UpdatedDate = DateTime.Now;
                db.Entry(blogEntry).State = EntityState.Modified;
                db.SaveChanges();

                var newPost = db.Posts.AsNoTracking().FirstOrDefault(p => p.Id == blogEntry.Id);
                if (oldPost.Title != newPost.Title)
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string Slug)
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

        // POST: BlogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string Slug)
        {
            BlogEntry blogEntry = db.Posts.Find(Slug);
            blogEntry.Published = false;
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
