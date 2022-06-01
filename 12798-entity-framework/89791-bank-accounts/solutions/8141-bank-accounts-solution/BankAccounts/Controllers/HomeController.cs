using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankAccounts.Models;

namespace BankAccounts.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext dbContext;


    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        dbContext = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    // Creates a new user
    [HttpPost("create")]
    public IActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            if (dbContext.Users.Any(o => o.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View("Register");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);

            var newUser = dbContext.Users.Add(user).Entity;
            dbContext.SaveChanges();

            HttpContext.Session.SetInt32("userId", newUser.UserId);
            HttpContext.Session.SetString("firstName", newUser.FirstName);
            HttpContext.Session.SetString("lastName", newUser.LastName);

            return RedirectToAction("Index");
        }
        return View("Register");
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser user)
    {
        if (ModelState.IsValid)
        {
            User? toLogin = dbContext.Users?.FirstOrDefault(u => u.Email == user.EmailAttempt);

            if (toLogin == null)
            {
                ModelState.AddModelError("EmailAttempt", "Invalid Email/Password");
                return View("Login");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(user, toLogin.Password, user.PasswordAttempt);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("EmailAttempt", "Invalid Email/Password");
                return View("Login");
            }
            // Log user into session
            HttpContext.Session.SetInt32("userId", toLogin.UserId);
            return RedirectToAction("Index");
        }
        return View("Login");
    }

    [HttpGet("logout")]
    public RedirectToActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Register");
    }

    [HttpGet("")]
    public IActionResult Index()
    {

        // var loggedInUserId = HttpContext.Session.GetInt32("userId");
        User? loggedInUser = dbContext.Users.Include(u => u.Transactions)
            .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));

        if (loggedInUser != null)
        {
            ViewBag.User = loggedInUser;
            ViewBag.Transactions = dbContext.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .Where(t => t.UserId == loggedInUser.UserId);
            return View();
        }

        return RedirectToAction("Register");

    }
    [HttpPost("money")]
    public IActionResult Money(Transaction trans)
    {
        if (ModelState.IsValid)
        {
            dbContext.Transactions.Add(trans);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        User? loggedInUser = dbContext.Users.Include(u => u.Transactions)
                .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));

        if (loggedInUser != null)
        {
            ViewBag.User = loggedInUser;
            ViewBag.Transactions = dbContext.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .Where(t => t.UserId == loggedInUser.UserId);

        }

        return View("Index");

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
