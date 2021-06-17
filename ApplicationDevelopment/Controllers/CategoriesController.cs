using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext context;
        public CategoriesController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Categories
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles ="Staff")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var newCategory = new Category
            {
                Name = category.Name,
                Description=category.Description
            };

            context.Categories.Add(newCategory);
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ModelState.AddModelError("", "Category Name already exists");
                return View();
            }
            return RedirectToAction("Index");
        }

        //Delete
        [HttpGet]
        [Authorize(Roles ="Staff")]
        public ActionResult Delete(int id)
        {
            var categoryInDb = context.Categories.SingleOrDefault(p => p.Id == id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            context.Categories.Remove(categoryInDb);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //Edit
        [HttpGet]
        [Authorize(Roles ="Staff")]
        public ActionResult Edit(int id)
        {
            var categoryInDb = context.Categories.SingleOrDefault(p => p.Id == id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            return View(categoryInDb);
        }
        [HttpPost]
        [Authorize(Roles ="Staff")]
        public ActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var categoryInDb = context.Categories.SingleOrDefault(p => p.Id == category.Id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;
                context.SaveChanges();
                return RedirectToAction("Index"); 
            }
        [HttpGet]
        [Authorize(Roles ="Staff")]
        public ActionResult Details(int id)
        {
            var categoryInDb = context.Categories.SingleOrDefault(p => p.Id == id);
            if(categoryInDb == null)
            {
                return HttpNotFound();
            }
            return View(categoryInDb);
        }
    }
}