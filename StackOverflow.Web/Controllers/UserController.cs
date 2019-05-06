using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackOverflow.Data;

namespace StackOverflow.Web.Controllers
{
    public class UserController : Controller
    {
        private string _connectionString;
        private QuestionsTagsRepository _repo;
        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _repo = new QuestionsTagsRepository(_connectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            _repo.AddUser(user);
            return Redirect("/home/index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            bool isVerified = _repo.LoginVerify(user);
            if (isVerified)
            {
                var claims = new List<Claim>
                {
                    new Claim("user", user.Email)
                };
                HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

                return Redirect("/home/index");
            }
            else
            {
                return Redirect("/user/login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/user/login");
        }
    }
}