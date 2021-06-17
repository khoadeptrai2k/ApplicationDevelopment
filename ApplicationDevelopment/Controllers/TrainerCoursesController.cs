using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using ApplicationDevelopment.ViewModel;

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
                var trainerView = context.TrainerCourses.Where(y => y.TrainerId == trainerId).Include(p => p.Course).ToList();
                return View(trainerView);
            }
            return View("Login");
        }
        
        //Create
        [Authorize(Roles ="Staff")]
        [HttpGet]
        public ActionResult Create()
        {
            //get trainee
            var role = (from p in context.Roles where p.Name.Contains("Trainer") select p).FirstOrDefault();
            var users = context.Users.Where(t => t.Roles.Select(n => n.RoleId).Contains(role.Id)).ToList();
            //get Course
            var courses = context.Courses.ToList();
            var TrainerCourseVM = new TrainerCourseViewModel()
            {
                Courses = courses,
                Trainers = users,
                TrainerCourse = new TrainerCourse()
            };
            return View(TrainerCourseVM);
        }
        [Authorize(Roles ="Staff")]
        [HttpPost]
        public ActionResult Create(TrainerCourse trainerCourse)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var checkTrainerCourses = context.TrainerCourses.Any(p => p.TrainerId == trainerCourse.TrainerId &&
                                                                    p.CourseId == trainerCourse.CourseId);
            if (checkTrainerCourses == true)
            {
                return View("", "InValid");
            }
            var newTrainerCourse = new TrainerCourse
            {
                TrainerId = trainerCourse.TrainerId,
                CourseId = trainerCourse.CourseId
            };
            context.TrainerCourses.Add(newTrainerCourse);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //Edit
        [HttpGet]
        [Authorize(Roles="Staff")]
        public ActionResult Edit(int id)
        {
            var trainercourseInDb = context.TrainerCourses.SingleOrDefault(p => p.Id == id);
            if (trainercourseInDb==null)
            {
                return HttpNotFound();
            }
            var viewModel = new TrainerCourseViewModel
            {
                TrainerCourse = trainercourseInDb,
                Courses = context.Courses.ToList(),
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles ="Staff")]
        public ActionResult Edit(TrainerCourse trainerCourse)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var trainercourseInDb = context.TrainerCourses.SingleOrDefault(p => p.Id == trainerCourse.Id);
            if (trainercourseInDb == null)
            {
                return HttpNotFound();
            }
            trainercourseInDb.CourseId = trainerCourse.CourseId;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //Delete
        [Authorize(Roles ="Staff")]
        public ActionResult Delete(int id)
        {
            var trainercourseInDb = context.TrainerCourses.SingleOrDefault(p => p.Id == id);
            if (trainercourseInDb == null)
            {
                return HttpNotFound();
            }
            context.TrainerCourses.Remove(trainercourseInDb);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}