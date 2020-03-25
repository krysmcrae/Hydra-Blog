using KMBlog.Helpers;
using KMBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMBlog.Controllers
{
    public class HomeController : Controller
    {
        HomeHelper homie = new HomeHelper();
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Author = db.Users.Where(u => u.Id == db.Posts.FirstOrDefault().AuthorId).FirstOrDefault().DisplayName;

            var publishedEntries = db.Posts.Where(p => p.Published==true).ToList();
            ViewBag.size= homie.BuildAHouse();
            return View(publishedEntries);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}