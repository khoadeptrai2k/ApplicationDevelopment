using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{

    public class CategoriesController : Controller
    {
        private ApplicationDbContext context;
        public CategoriesController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Categories
        [Authorize(Roles = "Staff")]
        public ActionResult Index(string searchCategory)
        {
            var categories = context.Categories.ToList();
            if (!String.IsNullOrEmpty(searchCategory))
            {
                categories = categories.FindAll(p => p.Name.Contains(searchCategory));
            }
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
        public ActionResult Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(context.Categories.Any(p =>p.Name.Contains(model.Name)))
            {
                ModelState.AddModelError("Name", "Category Name Exists.");
                return View(model);
            }
            var newCategory = new Category
            {
                Name = model.Name,
                Description=model.Description
            };

            context.Categories.Add(newCategory);
            context.SaveChanges();
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