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
    [Route("quiz")]
    public class QuizController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public QuizController(UserManager<AppUser> userManager, DataDbContext context)
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
                        join ls in _context.Lessions on qt.Id equals ls.QuizTypeId
                        join lq in _context.LessionQuizzes on ls.Id equals lq.LesionId
                        join qz in _context.Quizzes on lq.QuizId equals qz.Id select new ListQuizViewModel() { 
                            QuestionName = qz.QuestionName,
                            CorrectAnswer = qz.CorrectAnswer,
                            InCorrectAnswer1 = qz.InCorrectAnswer1,
                            InCorrectAnswer2 = qz.InCorrectAnswer2,
                            InCorrectAnswer3 = qz.InCorrectAnswer3,
                            InCorrectAnswer4 = qz.InCorrectAnswer4,
                            InCorrectAnswer5 = qz.InCorrectAnswer5,
                            NoAnswer = qz.NoAnswer,
                            CreatedDate = qz.CreatedDate,
                            CreatedTime = qz.CreatedTime,
                            CreatedBy = qz.CreatedBy,
                            DeletedBy = qz.DeletedBy,
                            IsActive = qz.IsActive,
                            IsDeleted = qz.IsDeleted,
                            ModifiedBy = qz.ModifiedBy,
                            ModifiedDate = qz.ModifiedDate,
                            ModifiedTime = qz.ModifiedTime,
                            LessonName = ls.Name,
                            QuizTypeName = qt.Name
                        })
                        .Where(p => p.IsDeleted == false && p.CreatedBy == userId)
                        .OrderByDescending(p => p.ModifiedDate)
                        .ToList();

            return View(quiz);
        }


        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.QuizTypes = new SelectList(_context.QuizTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreateQuizViewModel model, Guid lessonId, Guid quizTypeId)
        {
            if (ModelState.IsValid)
            {
                ViewData["TypeId"] = new SelectList(_context.QuizTypes, "Id", "Name", quizTypeId);

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
        [Route("load-lesson")]
        public JsonResult LoadLesson (Guid id)
        {
            var lesson = _context.Lessions.Where(p => p.QuizTypeId == id).ToList();
            return Json(new SelectList(lesson, "Id", "Name"));
        } 
    }
}
