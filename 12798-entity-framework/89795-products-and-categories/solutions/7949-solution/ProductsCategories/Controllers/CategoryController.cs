using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCategories.Models;

namespace ProductsCategories.Controllers
{
    [Route("category")]
    public class CategoryController : Controller
    {
        private MyContext dbContext;
        public CategoryController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllCategories = dbContext.Categories;
            return View();
        }
        [HttpGet("{categoryId}")]
        public IActionResult Show(int categoryId)
        {
            var cModel = dbContext.Categories
                .Include(p => p.Associations)
                .ThenInclude(a => a.Product)
                .FirstOrDefault(p => p.CategoryId == categoryId);
            
            // All/Any
            var unrelatedProducts = dbContext.Products
                .Include(p => p.Associations)
                .Where(p => p.Associations.All(a => a.CategoryId != categoryId));
            ViewBag.Products = unrelatedProducts;
            return View(cModel);
        }
        [HttpPost("create")]
        public IActionResult Create(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(newCategory);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AllCategories = dbContext.Categories;
            return View("Index");
        }
    }
}