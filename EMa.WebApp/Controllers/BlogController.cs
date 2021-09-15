using EMa.Common.Helpers;
using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMa.WebApp.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(UserManager<AppUser> userManager, DataDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = _context.Blogs.Where(p => p.IsDeleted == false && p.CreatedBy == userId)
                .OrderBy(p => p.ModifiedDate)
                .ToList();

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
        public IActionResult Create(CreateBlogViewModel model, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                var thumbUploaded = UploadImage.UploadImageFile(files);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Blog createItem = new Blog()
                {
                    Title = model.Title,
                    Content = model.Content,
                    Thumbnail = thumbUploaded.ToString(),
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

                _context.Blogs.Add(createItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Update(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = _context.Blogs.FirstOrDefault(p => p.IsActive == true 
            && p.IsDeleted == false 
            && p.CreatedBy == userId 
            && p.Id == id);

            ViewBag.Title = item.Title;

            if(userId != item.CreatedBy) 
            {
                return Unauthorized();
            }

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        [HttpPost]
        [Route("update/{id}")]
        public IActionResult Update(UpdateBlogViewModel model, IFormFile files, Guid id)
        {
            if(id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var thumbUploaded = UploadImage.UploadImageFile(files);

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var data = _context.Blogs.Where(p => p.Id == id).AsNoTracking().FirstOrDefault();


                    if (thumbUploaded == null)
                    {
                        Blog updateItem = new Blog()
                        {
                            Id = id,
                            Title = model.Title,
                            Content = model.Content,
                            CreatedBy = data.CreatedBy,
                            CreatedDate = data.CreatedDate,
                            CreatedTime = data.CreatedTime,
                            Thumbnail = data.Thumbnail,
                            DeletedBy = null,
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedBy = userId,
                            ModifiedDate = DateTime.Now,
                            ModifiedTime = DateTime.Now
                        };
                        _context.Blogs.Update(updateItem);
                    }
                    else
                    {
                        Blog updateItem = new Blog()
                        {
                            Id = id,
                            Title = model.Title,
                            Thumbnail = thumbUploaded,
                            Content = model.Content,
                            CreatedBy = data.CreatedBy,
                            CreatedDate = data.CreatedDate,
                            CreatedTime = data.CreatedTime,
                            DeletedBy = null,
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedBy = userId,
                            ModifiedDate = DateTime.Now,
                            ModifiedTime = DateTime.Now
                        };
                        _context.Entry(updateItem).State = EntityState.Modified;
                    }

                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                } 
                
            }

            return View();
        }


        [HttpGet]
        [Route("hide/{postId}")]
        public async Task<IActionResult> ChangeStatus(Guid postId)
        {
            var blog = await _context.Blogs.FindAsync(postId);

            if(blog.IsActive == true)
            {
                blog.IsActive = false;
            } else
            {
                blog.IsActive = true;
            }

            _context.Update(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Route("delete/{postId}")]
        public async Task<IActionResult> Delete(Guid postId)
        {
            var blog = await _context.Blogs.FindAsync(postId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            blog.IsDeleted = true;
            blog.DeletedBy = userId;
            blog.CreatedTime = DateTime.Now;

            _context.Update(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
