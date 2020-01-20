namespace KMBlog.Migrations
{
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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
