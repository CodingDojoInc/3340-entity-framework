using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Register()
        {
            return View();
        }

        // Create
        [HttpPost("create")]
        public IActionResult Create(BankUser user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(o => o.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Register");
                }

                PasswordHasher<BankUser> hasher = new PasswordHasher<BankUser>();
                user.Password = hasher.HashPassword(user, user.Password);

                var newUser = dbContext.Users.Add(user).Entity;
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("userId", newUser.UserId);

                return RedirectToAction("Index", "Bank");
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
            if(ModelState.IsValid)
            {
                BankUser toLogin = dbContext.Users.FirstOrDefault(u => u.Email == user.EmailAttempt);
                if(toLogin == null)
                {
                    ModelState.AddModelError("EmailAttempt", "Invalid Email/Password");
                    return View("Login");
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, toLogin.Password, user.PasswordAttempt);
                if(result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("EmailAttempt", "Invalid Email/Password");
                    return View("Login");
                }
                // Log user into session
                HttpContext.Session.SetInt32("userId", toLogin.UserId);
                return RedirectToAction("Index", "Bank");
            }
            return View("Login");
        }
        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Register");
        }
        [HttpGet("success")]
        public IActionResult Success()
        {
            if(HttpContext.Session.GetInt32("userId") == null)
                return RedirectToAction("Register");
            return View();
        }
    }
}
