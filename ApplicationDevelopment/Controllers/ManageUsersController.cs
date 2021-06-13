using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersController : Controller
    {
        private ApplicationDbContext context;
        private UserManager<IdentityUser> _userManager;
        public ManageUsersController()
        {
            context = new ApplicationDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        }
        // GET: ManageUsers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult UsersWithRoles()
        {
            //declare variable usersWithRoles using the (FROM-IN) function
            //to define the query data source (Users).
            var usersWithRoles = (from user in context.Users
                                  select new // SELECT: indicates the data exported from the source
                                  {
                                      UserId = user.Id,
                                      Name = user.Name,
                                      Username = user.UserName,
                                      Emailaddress = user.Email,
                                      RoleNames = (from userRole in user.Roles //from-in function selects external data source
                                                   join role in context.Roles //join-in function selects the data source inside
                                                   on userRole.RoleId //Data columns are connected between 2 tables
                                                   equals role.Id //fetching the data using on condition (role.Id)
                                                   select role.Name).ToList() //Select the value you want to display
                                  }).ToList().Select(p => new UsersInRole() //Attaches the selected values to the viewmodel

                                  {
                                      UserId = p.UserId,
                                      Name = p.Name,
                                      UserName = p.Username,
                                      Email = p.Emailaddress,
                                      Role = string.Join(",", p.RoleNames) //(join) Combine the selected value above to display
                                  });
            var except_user = context.Users.Where(t => t.Id == "95f5b8f0-972a-4ea3-b660-5b10a3e1d1d6");
            var user_in_role = context.Users.Except(except_user).ToList();
            return View(user_in_role);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var appUser = context.Users.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ApplicationUser user)
        {
            var userInDb = context.Users.Find(user.Id);

            if (userInDb == null)
            {
                return View(user);
            }

            if (ModelState.IsValid)
            {
                userInDb.Name = user.Name;
                userInDb.UserName = user.UserName;
                userInDb.Phone = user.Phone;
                userInDb.WorkingPlace = user.WorkingPlace;
                userInDb.Email = user.Email;


                context.Users.AddOrUpdate(userInDb);
                context.SaveChanges();

                return RedirectToAction("UsersWithRoles");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            var userInDb = context.Users.SingleOrDefault(p => p.Id == id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            context.Users.Remove(userInDb);
            context.SaveChanges();

            return RedirectToAction("UsersWithRoles");

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Details(string id)
        {
            var usersInDb = context.Users.SingleOrDefault(p => p.Id == id);

            if (usersInDb == null)
            {
                return HttpNotFound();
            }

            return View(usersInDb);
        }
    }
}