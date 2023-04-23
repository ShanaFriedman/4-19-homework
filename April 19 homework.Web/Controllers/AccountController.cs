using April_19_homework.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace April_19_homework.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=AdLogin; Integrated Security=true;";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User u, string password)
        {
            UserRepository repo = new(_connectionString);
            repo.AddUser(u, password);
            
            return Redirect("/home/index");
        }

        public IActionResult Login()
        {
            string message = (string)TempData["message"] ?? null;
            
            return View(model: message);
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            UserRepository repo = new(_connectionString);
            User u = repo.Login(email, password);

            if (u == null)
            {
                TempData["message"] = "Invalid Login";
                return Redirect("/account/login");
            }
            
            var claims = new List<Claim>
            {
                new Claim("user", email) 
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();

            return Redirect("/home/index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }
    }
}
