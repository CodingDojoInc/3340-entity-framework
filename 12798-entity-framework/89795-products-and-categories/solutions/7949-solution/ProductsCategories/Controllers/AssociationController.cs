using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Models;
namespace ProductsCategories.Controllers
{
    public class AssociationController : Controller
    {
        private MyContext dbContext;
        public AssociationController(MyContext context)
        {
            dbContext = context;
        }
        [HttpPost("product/{isProduct}")]
        public IActionResult Create(Association newAssoc)
        {
           
            dbContext.Associations.Add(newAssoc);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}