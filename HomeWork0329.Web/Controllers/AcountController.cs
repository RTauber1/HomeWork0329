using HomeWork0329.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace HomeWork0329.Web.Controllers
{
    public class AcountController : Controller
    {
        string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ALlMyProjects;Integrated Security=true;";
        public IActionResult SighnUp()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult SighnUp(string password, string email, string name)
        {
            Repository repository = new Repository(_connectionString);
            bool correctEmail =repository.AddUser(name, email, password);
            if (correctEmail)
            {
                return Redirect("/acount/Login");
            }
            TempData["message"] = "This email is already taken;(";
            return Redirect("/acount/SighnUp");
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Repository repository= new Repository(_connectionString);
            User user = repository.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Eather your password or email is incorrect;(";
                return RedirectToAction("Login");
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/MyAccount");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
