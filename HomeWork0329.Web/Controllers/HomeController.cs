using HomeWork0329.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HomeWork0329.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeWork0329.Web.Controllers
{
    public class HomeController : Controller
    {
        string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ALlMyProjects;Integrated Security=true;";
        public IActionResult Index()
        {
            Repository repository = new Repository(_connectionString);
            IndexViewModel indexViewModel = new IndexViewModel();
            indexViewModel.Posts = repository.GetPosts();
            if (User.Identity.IsAuthenticated)
            {
                int id = repository.GetUsersId(User.Identity.Name);
                foreach (Posts p in indexViewModel.Posts)
                {
                    if (p.UserId == id)
                    {
                        p.IsUsers = true;
                    }
                }
            }
            
            return View(indexViewModel);
        }
        [Authorize]
        public IActionResult NewAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewAdd(string title, string phoneNumber, string description)
        {
            Repository repository = new Repository(_connectionString);
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            string email = User.Identity.Name;
            User user= repository.GetByEmail(email);
            repository.AddPost(title, phoneNumber, description, user.Id);
            return Redirect("/");
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            Repository repository = new Repository(_connectionString);
            MyAccountViewModel myAccountViewModel = new MyAccountViewModel();
            myAccountViewModel.UsersPosts = repository.GetUsersPosts(User.Identity.Name);
            return View(myAccountViewModel);
        }
        [Authorize]
        public IActionResult Erase(int Id)
        {
            Repository repository = new Repository(_connectionString);
            repository.Erase(Id);
            return Redirect("/");

        }


    }
}
