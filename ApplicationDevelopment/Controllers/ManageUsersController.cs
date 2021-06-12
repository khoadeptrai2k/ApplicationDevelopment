using ApplicationDevelopment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    public class ManageUsersController : Controller
    {
        private ApplicationDbContext context;
        private UserManager<IdentityUser> userManager;
        public ManageUsersController()
        {
            context = new ApplicationDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        }
        // GET: ManageUsers
        [HttpGet]
        [Authorize(Roles ="Admin")]
        


        public ActionResult Index()
        {
            return View();
        }
    }
}