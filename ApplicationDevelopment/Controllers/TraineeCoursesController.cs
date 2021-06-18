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
        //Create
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult Create()
        {
            //get trainee
            var role = (from p in context.Roles where p.Name.Contains("Trainee") select p).FirstOrDefault();
            var users = context.Users.Where(t => t.Roles.Select(n => n.RoleId).Contains(role.Id)).ToList();
            //get Course
            var courses = context.Courses.ToList();
            var TraineeCourseVM = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourse = new TraineeCourse()
            };
            return View(TraineeCourseVM);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult Create(TraineeCourse traineeCourse)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var checkTraineeCourses = context.TraineeCourses.Any(p => p.TraineeId == traineeCourse.TraineeId &&
                                                                    p.CourseId == traineeCourse.CourseId);
            if (checkTraineeCourses == true)
            {
                return View("", "InValid");
            }
            var newTraineeCourse = new TraineeCourse
            {
                TraineeId = traineeCourse.TraineeId,
                CourseId = traineeCourse.CourseId
            };
            context.TraineeCourses.Add(newTraineeCourse);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //Edit
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(int id)
        {
            var traineecourseInDb = context.TraineeCourses.SingleOrDefault(p => p.Id == id);
            if (traineecourseInDb == null)
            {
                return HttpNotFound();
            }
            var viewModel = new TraineeCourseViewModel
            {
                TraineeCourse = traineecourseInDb,
                Courses = context.Courses.ToList(),
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(TraineeCourse traineeCourse)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var traineecourseInDb = context.TraineeCourses.SingleOrDefault(p => p.Id == traineeCourse.Id);
            if (traineecourseInDb == null)
            {
                return HttpNotFound();
            }
            traineecourseInDb.CourseId = traineeCourse.CourseId;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //Delete
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int id)
        {
            var traineecourseInDb = context.TraineeCourses.SingleOrDefault(p => p.Id == id);
            if (traineecourseInDb == null)
            {
                return HttpNotFound();
            }
            context.TraineeCourses.Remove(traineecourseInDb);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}