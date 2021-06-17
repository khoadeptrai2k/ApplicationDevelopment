using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ApplicationDevelopment.Controllers
{ 
    public class CoursesController : Controller
    {
        private ApplicationDbContext context;
        public CoursesController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Course
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            var courses = context.Courses.Include(p => p.Category).ToList();
            return View(courses);
        }

        //Create Course
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            var viewModel = new CourseCategoryViewModel
            {
                Categories = context.Categories.ToList(),
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoryViewModel()
                {
                    Course = course,
                    Categories = context.Categories.ToList()
                };
                return View(viewModel);
            }
            //check existed
            if (context.Courses.Any(p=>p.Name==course.Name
                                && p.CategoryId==course.CategoryId))
            {
                return View("~/Views/Courses/CheckExists.cshtml");
            }
            var newCourse = new Course
            {
                Name = course.Name,
                CategoryId = course.CategoryId,
                DueDate = course.DueDate,
            };
            context.Courses.Add(newCourse);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Delete
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int id)
        {
            var courseInDb = context.Courses.SingleOrDefault(p => p.Id == id);
            if(courseInDb==null)
            {
                return HttpNotFound();
            }
            context.Courses.Remove(courseInDb);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Edit
        [HttpGet]
        [Authorize(Roles = "Staff")]

        public ActionResult Edit(int id)
        {
            var courseInDb = context.Courses.SingleOrDefault(p => p.Id == id);

            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CourseCategoryViewModel
            {
                Course = courseInDb,
                Categories = context.Categories.ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]

        public ActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var courseInDb = context.Courses.SingleOrDefault(p => p.Id == course.Id);

            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            courseInDb.Name = course.Name;
            courseInDb.CategoryId = course.CategoryId;
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        //Details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var courseInDb = context.Courses.SingleOrDefault(p => p.Id == id);

            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            return View(courseInDb);
        }
    }
}