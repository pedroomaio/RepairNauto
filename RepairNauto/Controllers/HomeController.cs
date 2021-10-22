using AutoRepair.Data;
using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace AutoRepair.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

       

        private readonly IUserRepository _userRepository;

        public HomeController(
            IUserRepository userRepository)

        {
            _userRepository = userRepository;
        }
       public IActionResult Index()
        {

            if (User.IsInRole("Mechanic"))
            {
                return RedirectToAction("Index", "Inspecion");
            }
            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Inspecion");
            }

            return View(_userRepository.GetAll().Where(p => p.Email == User.Identity.Name));
        }
        public IActionResult Jornal(int a1, int a2)
        {
            int a = a1 + a2;
            return View(a);
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
}
