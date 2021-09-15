using EMa.Common.Helpers;
using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EMa.API.Controllers
{
    [ApiController]
    [Route("quiz")]
    public class QuizController : Controller
    {
        private readonly DataDbContext _context;

        public QuizController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetAll()
        {
            return await _context.Quizzes.Where(p => p.IsActive == true && p.IsDeleted == false).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetId(Guid id)
        {
			var quizType = await _context.Quizzes.FindAsync(id);


			if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }
        [HttpGet("/GetQuiz/{idlesson}")]
        public async Task<ActionResult<Quiz>> Get(Guid idlesson)
        {
            //var quizType = await _context.Quizzes.FindAsync(id);
            List<Quiz> quizType = new List<Quiz>();
            Guid lesson = idlesson;
            List<LessionQuiz> lessionQuiz = new List<LessionQuiz>();
            foreach (var item in _context.LessionQuizzes)
            {
                if (item.LesionId == lesson)
                {
                    lessionQuiz.Add(item);
                }
            }
            foreach (var item in lessionQuiz)
            {
                quizType.Add(await _context.Quizzes.FindAsync(item.QuizId));
            }

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpGet("{id}/{idv}")]
        public async Task<ActionResult<Quiz>> Get(Guid id,Guid idv)
        {
            //var quizType = await _context.Quizzes.FindAsync(id);
            List<Quiz> quizType = new List<Quiz>();
            Guid lesson = id;
            string type = idv.ToString();
            List<LessionQuiz> lessionQuiz = new List<LessionQuiz>();
			foreach (var item in _context.LessionQuizzes)
			{
				if (item.QuestionType == type && item.LesionId == lesson)
				{
                    lessionQuiz.Add(item);
				}
            }
            foreach (var item in lessionQuiz)
			{
                quizType.Add(await _context.Quizzes.FindAsync(item.QuizId));
			}
            
            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpPost("")]
        public async Task<ActionResult<Quiz>> Post(CreateQuizViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Quiz createItem = new Quiz()
            {
                QuestionName = model.QuestionName,
                NoAnswer = model.NoAnswer,
                CorrectAnswer = model.CorrectAnswer,
                InCorrectAnswer1 = model.InCorrectAnswer1,
                InCorrectAnswer2 = model.InCorrectAnswer2,
                InCorrectAnswer3 = model.InCorrectAnswer3,
                InCorrectAnswer4 = model.InCorrectAnswer4,
                InCorrectAnswer5 = model.InCorrectAnswer5,
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
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateQuizViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Quiz updateItem = new Quiz()
            {
                Id = id,
                QuestionName = model.QuestionName,
                NoAnswer = model.NoAnswer,
                CorrectAnswer = model.CorrectAnswer,
                InCorrectAnswer1 = model.InCorrectAnswer1,
                InCorrectAnswer2 = model.InCorrectAnswer2,
                InCorrectAnswer3 = model.InCorrectAnswer3,
                InCorrectAnswer4 = model.InCorrectAnswer4,
                InCorrectAnswer5 = model.InCorrectAnswer5,
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
            _context.Entry(updateItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Quiz>> Delete(Guid id)
        {
            var item = await _context.Quizzes.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Quizzes.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("hide/{id}")]
        public async Task<IActionResult> Hide(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Quiz updateItem = new Quiz()
            {
                Id = id,
                CreatedDate = DateTime.Now,
                CreatedTime = DateTime.Now,
                CreatedBy = userId,
                DeletedBy = userId,
                IsActive = true,
                IsDeleted = true,
                ModifiedBy = userId,
                ModifiedDate = DateTime.Now,
                ModifiedTime = DateTime.Now
            };
            _context.Entry(updateItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateItem);
        }

        private bool CheckExists(Guid id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}
