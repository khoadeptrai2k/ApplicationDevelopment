using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    public class StaffManageUsersController : Controller
    {
        private ApplicationDbContext context;
        public StaffManageUsersController()
        {
            context = new ApplicationDbContext();
        }
        // GET: StaffManageUsers
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            var UserInfos = (from user in context.Users
                             select new
                             {
                                 UserId = user.Id,
                                 Username = user.UserName,
                                 EmailAddress = user.Email,
                                 RoleName = (from userRole in user.Roles
                                             join role in context.Roles
                                             on userRole.RoleId
                                             equals role.Id
                                             select role.Name)
                             }
                       ).ToList().Select(p => new UsersInRole()
                       {
                           UserId = p.UserId,
                           UserName = p.Username,
                           Email = p.EmailAddress,
                           Role = string.Join(",", p.RoleName)
                       }
                       );
            return View(UserInfos);
        }


        //Edit
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(string id)
        {
             var AccountInDb = context.Users.SingleOrDefault(p => p.Id == id);
             if (AccountInDb == null)
                 {
                     return HttpNotFound();
                 }
                 return View(AccountInDb);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var AccountInDb = context.Users.SingleOrDefault(p => p.Id == user.Id);
            if (AccountInDb == null)
            {
                return HttpNotFound();
            }
            AccountInDb.UserName = user.UserName;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Delete
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(string id)
        {
            var AccountInDb = context.Users.SingleOrDefault(p => p.Id == id);
            if (AccountInDb == null)
            {
                return HttpNotFound();
            }
            context.Users.Remove(AccountInDb);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}