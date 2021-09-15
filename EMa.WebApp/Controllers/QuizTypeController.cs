using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMa.WebApp.Controllers
{
    [Route("quiz-type")]
    public class QuizTypeController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public QuizTypeController(UserManager<AppUser> userManager, DataDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var quiz = _context.QuizTypes.Where(p => p.IsDeleted == false && p.CreatedBy == userId)
                .OrderBy(p => p.ModifiedDate)
                .ToList();

            return View(quiz);
        }

        [HttpGet]
        [Route("add-lesson/{quizTypeId}")]
        public IActionResult AddLesson(Guid? quizTypeId)
        {
            if(quizTypeId == null)
            {
                return NotFound();
            }

            var quizType = _context.QuizTypes.FirstOrDefault(p => p.Id == quizTypeId);

            ViewBag.QuizTypeName = quizType.Name;
            ViewBag.QuizTypeId = quizType.Id;

            return View();
        }


        [HttpPost]
        [Route("add-lesson/{quizTypeId}")]
        public IActionResult AddLesson(CreateLessonViewModel model, Guid quizTypeId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Lession createItem = new Lession()
                {
                    QuizTypeId = model.QuizTypeId,
                    Name = model.Name,
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

                _context.Lessions.Add(createItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreateQuizTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                QuizType createItem = new QuizType()
                {
                    Name = model.Name,
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

                _context.QuizTypes.Add(createItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = _context.QuizTypes.FirstOrDefault(p => p.IsActive == true
            && p.IsDeleted == false
            && p.CreatedBy == userId
            && p.Id == id);

            ViewBag.Name = item.Name;

            if (userId != item.CreatedBy)
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
        public IActionResult Update(UpdateQuizTypeViewModel model, Guid id)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var data = _context.QuizTypes.Where(p => p.Id == id).AsNoTracking().FirstOrDefault();

                    QuizType updateItem = new QuizType()
                    {
                        Id = id,
                        Name = model.Name,
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
                    _context.QuizTypes.Update(updateItem);

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
        [Route("hide/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            var item = await _context.QuizTypes.FindAsync(id);

            if (item.IsActive == true)
            {
                item.IsActive = false;
            }
            else
            {
                item.IsActive = true;
            }

            _context.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.QuizTypes.FindAsync(id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            item.IsDeleted = true;
            item.DeletedBy = userId;
            item.CreatedTime = DateTime.Now;

            _context.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
