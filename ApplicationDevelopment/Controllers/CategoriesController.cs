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
        public ActionResult Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var category = new Category
            {
                Name = model.Name
            };

            context.Categories.Add(category);
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ModelState.AddModelError("", "Category Name already exists");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}