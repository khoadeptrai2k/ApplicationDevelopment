using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModel;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
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

        [Authorize(Roles = "Admin")]
        public ActionResult ResetPassword(string id)
        {
            // Declare the userId variable of Current.User.Identity and access the Id field through GetUserId
            var AccountInDB = context.Users.SingleOrDefault(p => p.Id == id);

            if (AccountInDB == null)
            {
                return HttpNotFound();
            }
            if (AccountInDB.Id != null)
            {
                // userManager by managing new users, bringing new data
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                // Delete current password of userManager
                userManager.RemovePassword(AccountInDB.Id);
                // Replace new password "Tukhoa@123" for userManager
                String newPassword = "Tukhoa@123";
                userManager.AddPassword(AccountInDB.Id, newPassword);
            }
            context.SaveChanges();
            return RedirectToAction("UsersWithRoles", "ManageUsers");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeUserPassword(string id)
        {
            var user = context.Users.SingleOrDefault(t => t.Id == id);
            var ChangePass = new AdminChangePassViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
            };
            return View(ChangePass);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeUserPassword(AdminChangePassViewModel model)
        {
            var user = context.Users.Where(t => t.Id == model.UserId).First();
            if (user.PasswordHash != null)
            {
                await _userManager.RemovePasswordAsync(user.Id);
            }
            await _userManager.AddPasswordAsync(user.Id, model.NewPassword);
            return RedirectToAction("UsersWithRoles", "ManageUsers");
        }
    }
}