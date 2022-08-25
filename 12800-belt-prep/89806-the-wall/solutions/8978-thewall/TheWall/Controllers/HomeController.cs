using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using TheWall.Models;

namespace TheWall.Controllers;

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
        return View();
    }

    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
        if(ModelState.IsValid)
        {
            if(_context.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View("Index");
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("Wall");
        } else {
            return View("Index");
        }
    }

    [HttpPost("login")]
    public IActionResult Login(LogUser returnUser)
    {
        if(ModelState.IsValid)
        {
            User? userInDb = _context.Users.FirstOrDefault(a => a.Email == returnUser.LogEmail);
            if(userInDb == null)
            {
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View("Index");
            }
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            var result = hasher.VerifyHashedPassword(returnUser, userInDb.Password, returnUser.LogPassword);
            if(result == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("Wall");
        } else {
            return View("Index");
        }
    }

    [HttpGet("Wall")]
    public IActionResult Wall()
    {
        if(HttpContext.Session.GetInt32 == null)
        {
            return RedirectToAction("Index");
        }
        ViewBag.User = _context.Users.FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("UserId"));
        ViewBag.AllMessages = _context.Messages.Include(d => d.Writer).Include(s => s.CommentsOnMessage).ThenInclude(d => d.Commenter).ToList();
        return View();
    }

    [HttpPost("message/create")]
    public IActionResult NewMessage(Message newMessage)
    {
        if(HttpContext.Session.GetInt32 == null)
        {
            return RedirectToAction("Index");
        }
        if(ModelState.IsValid)
        {
            newMessage.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("Wall");
        } else {
            ViewBag.User = _context.Users.FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            ViewBag.AllMessages = _context.Messages.Include(d => d.Writer).Include(s => s.CommentsOnMessage).ThenInclude(d => d.Commenter).ToList();
            return View("Wall");
        }
    }

    [HttpPost("comment/create/{messId}")]
    public IActionResult NewComment(Comment newComment, int messId)
    {
        if(HttpContext.Session.GetInt32 == null)
        {
            return RedirectToAction("Index");
        }
        if(ModelState.IsValid)
        {
            newComment.UserId = (int)HttpContext.Session.GetInt32("UserId");
            newComment.MessageId = messId;
            _context.Add(newComment);
            _context.SaveChanges();
            return RedirectToAction("Wall");
        } else {
            ViewBag.User = _context.Users.FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            ViewBag.AllMessages = _context.Messages.Include(d => d.Writer).Include(s => s.CommentsOnMessage).ThenInclude(d => d.Commenter).ToList();
            return View("Wall");
        }
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
