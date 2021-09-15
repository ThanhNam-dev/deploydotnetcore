using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace EMa.WebApp.Controllers
{
    [Route("post")]
    public class PostController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PostController(UserManager<AppUser> userManager, DataDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = _context.Posts.Where(p => p.IsActive == true && p.IsDeleted == false 
            && p.CreatedBy == userId).ToList();

            var postCommentNo = (from p in _context.Posts
                          join c in _context.PostComments on p.Id equals c.PostId select c.Id).Count();

            var postLikedNo = (from p in _context.Posts
                                 join c in _context.PostLikes on p.Id equals c.PostId
                                 select c.Id).Count();

            ViewBag.PostCommentNo = postCommentNo;
            ViewBag.PostLikedNo = postLikedNo;

            return View(post);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreatePostViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Post createItem = new Post()
                {
                    UserId = Guid.Parse(userId),
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    CreatedTime = DateTime.Now,
                    CreatedBy = userId,
                    DeletedBy = null,
                    IsActive = true,
                    IsDeleted = false,
                    ModifiedBy = userId,
                    ModifiedDate = DateTime.Now,
                    ModifiedTime = DateTime.Now
                };

                _context.Posts.Add(createItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
