using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using landing.Models;

namespace landing.Controllers
{
    public class HomeController : Controller
    {
        private bool inSession
        {
            get { return HttpContext.Session.GetInt32("UserId") != null; }
        }
        private User loggedInUser
        {
            get { return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId")); }
        }
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("admin/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(o=>o.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                    return View("Register");
                }

                PasswordHasher<User> hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);

                var newUser = dbContext.Users.Add(user).Entity;
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("AdminPanel");
            }
        return View("Register");
        }

        [HttpGet("admin/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("admin/login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                User toLogin = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if(toLogin == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                PasswordHasher<LoginUser> hasher  = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, toLogin.Password, user.Password);
                if(result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("UserId", toLogin.UserId);
                return RedirectToAction("AdminPanel");
            }
            return View("Login");
        }

        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet("adminpanel")]
        public IActionResult AdminPanel()
        {
            if(!inSession)
            {
                return RedirectToAction("Login");
            }
            var user = loggedInUser;
            ViewBag.User = user;

            ViewBag.Customers = dbContext.Customers
                .OrderByDescending(c => c.CreatedAt);

            return View("AdminPanel");
        }

        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        [HttpPost("signup")]
        public IActionResult SignUp(Customer newCustomer)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Customers.Any(o => o.Email == newCustomer.Email))
                {
                    ModelState.AddModelError("Email", "Email has already been used");
                    return View("SignUp");
                }
                var newCust = dbContext.Customers.Add(newCustomer).Entity;
                dbContext.SaveChanges();
                return RedirectToAction("Thanks");
            }
        return View("SignUp");
        }

        [HttpGet("thankyou")]
        public IActionResult Thanks()
        {
            return View("Thanks");
        }
    }
}
