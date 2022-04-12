using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCategories.Models;

namespace ProductsCategories.Controllers
{
    public class ProductController : Controller
    {
        private MyContext dbContext;
        public ProductController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllProducts = dbContext.Products;
            return View();
        }
        [HttpGet("{productId}")]
        public IActionResult Show(int productId)
        {
            var pModel = dbContext.Products
                .Include(p => p.Associations)
                .ThenInclude(a => a.Category)
                .FirstOrDefault(p => p.ProductId == productId);
            
            // All/Any
            var unrelatedCategories = dbContext.Categories
                .Include(c => c.Associations)
                .Where(c => c.Associations.All(a => a.ProductId != productId));

            // value of most expensive
            float mostExpensiveValue = dbContext.Products.Max(pr => pr.Price);

            // find most expensive product
            Product mostExpensive = dbContext.Products
                .FirstOrDefault(p => p.Price == mostExpensiveValue);

            // find first category that is associated with the most expensive product
            Category associatedToMostExpensive = dbContext.Categories
                .Include(c => c.Associations)
                .ThenInclude(a => a.Product)
                .FirstOrDefault(c => c.Associations.Any(a => a.Product.Price == mostExpensiveValue));
            ViewBag.Categories = unrelatedCategories;
            return View(pModel);
        }
        [HttpPost("create")]
        public IActionResult Create(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                dbContext.Products.Add(newProduct);
                dbContext.SaveChanges();
            }
            ViewBag.AllProducts = dbContext.Products;
            return View("Index");
        }
    }
}