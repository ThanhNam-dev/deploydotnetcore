using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMa.WebApp.Controllers
{
    [Route("lesson")]
    public class LessonController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LessonController(UserManager<AppUser> userManager, DataDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var quiz = (from qt in _context.QuizTypes
                     join ls in _context.Lessions on qt.Id equals ls.QuizTypeId select new ListLessonViewModel() { 
                         Id = ls.Id,
                         Name = ls.Name,
                         QuizName = qt.Name,
                         QuizTypeId = ls.QuizTypeId,
                         CreatedDate = ls.CreatedDate,
                         CreatedTime = ls.CreatedTime,
                         CreatedBy = ls.CreatedBy,
                         DeletedBy = ls.DeletedBy,
                         IsActive = ls.IsActive,
                         IsDeleted = ls.IsDeleted,
                         ModifiedBy = ls.ModifiedBy,
                         ModifiedDate = ls.ModifiedDate,
                         ModifiedTime = ls.ModifiedTime
                     })
                     .Where(p => p.IsDeleted == false && p.CreatedBy == userId)
                     .OrderByDescending(p => p.ModifiedDate)
                     .ToList();

            return View(quiz);
        }

        [HttpGet]
        [Route("add-quiz/{lessonId}")]
        public IActionResult AddQuiz(Guid? lessonId)
        {
            if (lessonId == null)
            {
                return NotFound();
            }

            var lession = _context.Lessions.FirstOrDefault(p => p.Id == lessonId);

            ViewBag.LessonName = lession.Name;
            ViewBag.LessonId = lession.Id;

            return View();
        }


        [HttpPost]
        [Route("add-quiz/{lessonId}")]
        public IActionResult AddQuiz(CreateQuizViewModel model, Guid lessonId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Quiz createItem = new Quiz()
                {
                    QuestionName = model.QuestionName,
                    CorrectAnswer = model.CorrectAnswer,
                    InCorrectAnswer1 = model.InCorrectAnswer1,
                    InCorrectAnswer2 = model.InCorrectAnswer2,
                    InCorrectAnswer3 = model.InCorrectAnswer3,
                    InCorrectAnswer4 = model.InCorrectAnswer4,
                    InCorrectAnswer5 = model.InCorrectAnswer5,
                    NoAnswer = model.NoAnswer,
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

                _context.Quizzes.Add(createItem);
                _context.SaveChanges();

                LessionQuiz lessionQuiz = new LessionQuiz()
                {
                    LesionId = lessonId,
                    QuizId = createItem.Id,
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
                _context.LessionQuizzes.Add(lessionQuiz);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.QuizTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreateLessonViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewData["TypeId"] = new SelectList(_context.QuizTypes, "Id", "Name", model.QuizTypeId);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Lession createItem = new Lession()
                {
                    Name = model.Name,
                    QuizTypeId = model.QuizTypeId,
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
    }
}
