namespace KMBlog.Migrations
{
    using KMBlog.Enum;
    using KMBlog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            #region Role Management Section 
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            #endregion
            #region User Management Section
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "krysmcrae@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "krysmcrae@Mailinator.com",
                    UserName = "krysmcrae@Mailinator.com",
                    FirstName = "Krys",
                    LastName = "McRae",
                    DisplayName = "kmac"
                }, "Abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "TestModerator@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "TestModerator@Mailinator.com",
                    UserName = "TestModerator@Mailinator.com",
                    FirstName = "Test",
                    LastName = "Case",
                    DisplayName = "Tester"
                }, "Abc&123!");
            }
            #endregion
            #region Role Assignment
            var adminId = userManager.FindByEmail("krysmcrae@Mailinator.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var modId = userManager.FindByEmail("TestModerator@Mailinator.com").Id;
            userManager.AddToRole(modId, "Moderator");
            #endregion

            #region Seed Blog Posts

            #region Everything's Marked Up
            if (!context.Posts.Any(t => t.Title == "Everything's Marked Up!"))
            {
                context.Posts.Add(new BlogEntry
                {
                    Title = "Everything's Marked Up!",
                    AuthorId = context.Users.Where(u => u.FirstName == "Krys").FirstOrDefault().Id,
                    Slug = "html",
                    Abstract = "\"HTML is used to provide structure and hierarchy within the format and display of a webpages content\"",
                    AbstractImageURL = "/Images/htmlAbstractImage.jpg",
                    Body1 = "HTML...You've heard of it, you may even know what it can do, but do you understand it?" +
                    " <span style=\"color:#F6490D;\">H</span>yper<span style=\"color:#F6490D;\">t</span>ext <span style=\"color:#F6490D;\">M</span>arkup <span style=\"color:#F6490D;\">L</span>anguage " +
                    "utilizes a blank web\"page\" and annotates the page with directions for the content. Those directions come in the form of tags, structured usually &lt;openingtag&gt;" +
                    "CONTENT_WITHIN_ELEMENT&lt;/closingtag&gt;. \"openingtag\" and \"closingtag\" would be replaced by whatever element is in use",
                    Body2 = "Some of the most common tags are as follows: < br />" +
                    "  < ul >" +
                    "    < li > &lt; h & gt; Heading </ li > < br>" +
                    "    < li > &lt; a & gt; Anchor(hyperlink) </ li > < br />" +
                    "  < li > &lt; img & gt; Image </ li > < br /> " +
                    " < li > &lt; div & gt; Division(section) </ li > < br /> " +
                    "< li > &lt; p & gt; Paragraph </ li > < br />" +
                    " < li > &lt; br / &gt; Line Break </ li > < br />" +
                    "< li > &lt; !-- --&gt; Comments </ li >" +
                    "</ ul >",
                    Body3 = "On top of Vanilla HTML, there are preprocessors available. A preprocessor is a " +
                    "program that takes one type of data and converts it to another type of data. In the case of HTML, " +
                    "some of the more popular preprocessor languages include Haml and Pug(formerly known as Jade). These preprocessors were built to help remove the inefficiencies and extend the logic of designing a page.",
                    CreationDate = DateTime.Parse("1/8/2020 8:30:11 AM"),
                    MediaURL = "UB1O30fR-EE",
                    PostType = PostType.MVC,
                    Published = true

                });
            }

            #endregion
            #region  Design's Come Cascading Down
            if (!context.Posts.Any(t => t.Title == "Design's Come Cascading Down"))
            {
                context.Posts.Add(new BlogEntry
                {
                    Title = "Design's Come Cascading Down",
                    AuthorId = context.Users.Where(u => u.FirstName == "Krys").FirstOrDefault().Id,
                    Slug = "css",
                    Abstract = "\"CSS is used to describe the look and feel of a markup document, providing a separation of concerns with a webpages content and it's presentation\"",
                    AbstractImageURL = "/Images/cssAbstractImage.png",
                    Body1 = "CSS...another acronym, another language, so what makes this one necessary? CSS is used to describe the look and feel of a markup document, providing a separation of concerns with a webpages content and it's presentation." +
                    " <span style=\"color:#F6490D;\">C</span>ascading<span style=\"color:#F6490D;\">S</span>tyle <span style=\"color:#F6490D;\">S</span>heet " +
                    "utilizes selectors based off of the HTML document and applies individual styles, or properties. Common selectors are the various element tags, classes and even the id assigned to a specific element.",
                    Body2 = "Element declarations have no prefix: < br />" +
                    "h1 { font - size: large;} < br />< h1 > Hello! </ h1 >< br />< br />Class declarations are prefixed by ‘.’: < br /> .header { font - size: large; }< br /> < h1 class=“header”>Hello!</h1>< br />Id declarations are prefixed by ‘#’:< br />" +
                    "#hello { font-size: large; }< br />< h1 id =“hello”>	Hello!</h1> < br />",
                    Body3 = "Styles can be added inline HTML with the style attribute <br/>Ex: < div style =“font - size: large;”> <br />" +
                    "Additionally, CSS uses preprocessors as well. With regards to CSS, some of the more popular preprocessor languages include SASS and SCSS.",
                    CreationDate = DateTime.Parse("1/10/2020 2:10:11 PM"),
                    MediaURL = "yfoY53QXEnI",
                    PostType = PostType.MVC,
                    Published = true

                });
            }

            #endregion
            #region Coffee is to Espresso like Java is to Javascript...Right?!
            if (!context.Posts.Any(t => t.Title == "Coffee is to Espresso like Java is to Javascript...Right?!"))
            {
                context.Posts.Add(new BlogEntry
                {
                    Title = "Coffee is to Espresso like Java is to Javascript...Right?!",
                    AuthorId = context.Users.Where(u => u.FirstName == "Krys").FirstOrDefault().Id,
                    Slug = "javascript",
                    Abstract = "\"While they do share a lot of the same syntax, that's where the similarities end.\"",
                    AbstractImageURL = "/Images/javascriptAbstractImage.png",
                    Body1 = "Javascript, it's a light version of Java right? One that can be used on browsers and web applications, versus Java for desktop applicatins. That made for an easy article...WRONG!" +
                    " While they do share a lot of the same syntax, that's where the similarities end." +
                    "utilizes a blank web\"page\" and annotates the page with directions for the content. Those directions come in the form of tags, structured usually &lt;openingtag&gt;" +
                    "CONTENT_WITHIN_ELEMENT&lt;/closingtag&gt;. \"openingtag\" and \"closingtag\" would be replaced by whatever element is in use",
                    Body2 = "",
                    Body3 = "",
                    CreationDate = DateTime.Parse("1/14/2020 10:27:11 AM"),
                    MediaURL = "hdI2bqOjy3c",
                    PostType = PostType.MVC,
                    Published = true

                });
            }

            #endregion
       

            #endregion
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
