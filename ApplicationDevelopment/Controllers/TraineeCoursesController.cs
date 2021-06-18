using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace ApplicationDevelopment.Controllers
{
    public class TraineeCoursesController : Controller
    {
        private ApplicationDbContext context;
        public TraineeCoursesController()
        {
            context = new ApplicationDbContext();
        }
        // GET: TraineeCourses
        public ActionResult Index()
        {
            if (User.IsInRole("Staff"))
            {
                var traineecourses = context.TraineeCourses.Include(p => p.Course).Include(m => m.Trainee).ToList();
                return View(traineecourses);
            }
            if (User.IsInRole("Trainee"))
            {
                var traineeId = User.Identity.GetUserId();
                var traineeView = context.TraineeCourses.Where(p => p.TraineeId == traineeId).Include(m => m.Course).ToList();
                return View(traineeView);
            }
            return View("Login");
        }
    }
}