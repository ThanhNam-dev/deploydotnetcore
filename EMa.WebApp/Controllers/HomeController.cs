using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMa.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, 
            UserManager<AppUser> userManager, 
            DataDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var topBlog = _context.Blogs.Where(p => p.IsActive == true && p.IsDeleted == false && p.CreatedBy == userId)
                .Take(3)
                .ToList();

            ViewBag.Top3Blog = topBlog;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
