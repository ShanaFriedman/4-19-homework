using April_19_homework.Data;
using April_19_homework.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace April_19_homework.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=AdLogin; Integrated Security=true;";

        public IActionResult Index()
        {
            HomeViewModel vm = new();
            var repo = new UserRepository(_connectionString);
            if (User.Identity.IsAuthenticated)
            {
                var currentUserEmail = User.Identity.Name;
                vm.User = repo.GetByEmail(currentUserEmail);
            };

            vm.Ads = repo.GetAds(0);

            return View(vm);
        }
        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewAd(Ad a)
        {
            var repo = new UserRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            User u = repo.GetByEmail(currentUserEmail);
            
            repo.NewAd(a, u.Id);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            var repo = new UserRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            User u = repo.GetByEmail(currentUserEmail);
            List<Ad> ads = repo.GetAds(u.Id);
            return View(ads);
        }

        [HttpPost]
        public IActionResult DeleteAd(int Id)
        {
            var repo = new UserRepository(_connectionString);
            repo.DeleteAd(Id);
            return RedirectToAction("index");
        }
        
    }
}