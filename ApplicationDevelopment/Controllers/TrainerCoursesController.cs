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
    public class TrainerCoursesController : Controller
    {
        private ApplicationDbContext context;
        public TrainerCoursesController()
        {
            context = new ApplicationDbContext();
        }
        // GET: TrainerCourses
        public ActionResult Index()
        {
            if (User.IsInRole("Staff"))
            {
                var trainercourses = context.TrainerCourses.Include(p => p.Course).Include(p => p.Trainer).ToList();
                return View(trainercourses);
            }
            if (User.IsInRole("Trainer"))
            {
                var trainerId = User.Identity.GetUserId();
                var trainerView = context.TrainerCourses.Where(p => p.TrainerId == trainerId).Include(p => p.Course).ToList();
                return View(trainerView);
            }
            return View("Login");
        }
    }
}