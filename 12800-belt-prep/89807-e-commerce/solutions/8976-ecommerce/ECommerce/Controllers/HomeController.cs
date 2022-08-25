using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;

namespace ECommerce.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context; 
    }

    public IActionResult Index()
    {
        ViewBag.AllProducts = _context.Products.Take(3).ToList();
        ViewBag.RecentCustomers = _context.Customers.Take(3).ToList();
        ViewBag.RecentOrders = _context.Orders.Include(s => s.Customer).Include(s => s.Product).Take(3).ToList();
        return View();
    }

    [HttpGet("products")]
    public IActionResult Products()
    {
        ViewBag.AllProducts = _context.Products.ToList();
        return View();
    }

    [HttpPost("products/search")]
    public IActionResult Search(string Filter)
    {
        if(Filter == "" || Filter == null)
        {
            return RedirectToAction("Products");
        } else {
            return RedirectToAction("Filtered", new {filter = Filter});
        }
    }

    [HttpGet("products/search/{filter}")]
    public IActionResult Filtered(string filter)
    {
        ViewBag.AllProducts = _context.Products.Where(a => a.Name.Contains(filter)).ToList();
        return View("Products");
    }

    [HttpPost("products/new")]
    public IActionResult NewProduct(Product newProd)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newProd);
            _context.SaveChanges();
            return RedirectToAction("Products");
        } else {
            ViewBag.AllProducts = _context.Products.ToList();
            return View("Products");
        }
    }

    [HttpGet("orders")]
    public IActionResult Orders()
    {
        ViewBag.AllCustomers = _context.Customers.OrderBy(a => a.Name).ToList();
        ViewBag.AllProducts = _context.Products.OrderBy(a => a.Name).ToList();
        ViewBag.AllOrders = _context.Orders.Include(s => s.Customer).Include(s => s.Product).OrderBy(a => a.CreatedAt).ToList();
        return View();
    }

    [HttpPost("orders/new")]
    public IActionResult NewOrder(Order newOrder)
    {
        if(ModelState.IsValid)
        {
            Product? ProductBought = _context.Products.FirstOrDefault(a => a.ProductId == newOrder.ProductId);
            if(ProductBought == null)
            {
                return RedirectToAction("Index");
            }
            if(ProductBought.Quantity < newOrder.QuantityBought)
            {
                ModelState.AddModelError("QuantityBought", "Not enough product in stock!");
                ViewBag.AllCustomers = _context.Customers.OrderBy(a => a.Name).ToList();
                ViewBag.AllProducts = _context.Products.OrderBy(a => a.Name).ToList();
                ViewBag.AllOrders = _context.Orders.ToList();
                return View("Orders");
            }
            ProductBought.Quantity = ProductBought.Quantity - newOrder.QuantityBought;
            _context.Add(newOrder);
            _context.SaveChanges();
            return RedirectToAction("Orders");
        } else {
            ViewBag.AllCustomers = _context.Customers.OrderBy(a => a.Name).ToList();
            ViewBag.AllProducts = _context.Products.OrderBy(a => a.Name).ToList();
            ViewBag.AllOrders = _context.Orders.ToList();
            return View("Orders");
        }
    }

    [HttpGet("customers")]
    public IActionResult Customers()
    {
        ViewBag.AllCustomers = _context.Customers.ToList();
        return View();
    }

    [HttpPost("customers/new")]
    public IActionResult NewCustomer(Customer newCust)
    {
        if(ModelState.IsValid)
        {
            if(_context.Customers.Any(a => a.Name == newCust.Name))
            {
                ModelState.AddModelError("Name", "That customer name already exists!");
                ViewBag.AllCustomers = _context.Customers.ToList();
                return View("Customers");
            }
            _context.Add(newCust);
            _context.SaveChanges();
            return RedirectToAction("Customers");
        } else {
            ViewBag.AllCustomers = _context.Customers.ToList();
            return View("Customers");
        }
    }

    [HttpPost("customers/delete/{custId}")]
    public IActionResult DeleteCustomer(int custId)
    {
        Customer? custToDelete = _context.Customers.FirstOrDefault(a => a.CustomerId == custId);
        if(custToDelete == null)
        {
            return RedirectToAction("Index");
        } else {
            _context.Remove(custToDelete);
            _context.SaveChanges();
            return RedirectToAction("Customers");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
