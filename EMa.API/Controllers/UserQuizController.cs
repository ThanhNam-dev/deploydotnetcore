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
    [Route("userQuiz")]
    public class UserQuizController : Controller
    {
        private readonly DataDbContext _context;

        public UserQuizController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<UserQuiz>>> GetAll()
        {
            return await _context.UserQuizzes.Where(p => p.IsActive == true && p.IsDeleted == false).ToListAsync();
        }

        [HttpGet("getId")]
        public async Task<ActionResult<UserQuiz>> Get()
        {
            List<UserQuiz> userQuiz = new List<UserQuiz>();
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            foreach (var item in _context.UserQuizzes)
            {
				if (item.UserId == Guid.Parse(userId))
				{
                    userQuiz.Add(item);
				}
            }

            if (userQuiz == null)
            {
                return NotFound();
            }

            return Ok(userQuiz);
        }

        [HttpPost("")]
        public async Task<ActionResult<UserQuiz>> Post(CreateUserQuizViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            UserQuiz createItem = new UserQuiz()
            {
                QuizId = model.QuizId,
                UserId = Guid.Parse(userId),
                SubmittedAt = DateTime.Now,
                Answer = model.Answer,
                RightOrWrong = model.RightOrWrong,
                NoExams = model.NoExams,
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

            _context.UserQuizzes.Add(createItem);
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateUserQuizViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            UserQuiz updateItem = new UserQuiz()
            {
                Id = id,
                QuizId = model.QuizId,
                UserId = model.UserId,
                SubmittedAt = model.SubmittedAt,
                Answer = model.Answer,
                RightOrWrong = model.RightOrWrong,
                NoExams = model.NoExams,
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
        public async Task<ActionResult<UserQuiz>> Delete(Guid id)
        {
            var item = await _context.UserQuizzes.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.UserQuizzes.Remove(item);
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

            UserQuiz updateItem = new UserQuiz()
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
            return _context.UserQuizzes.Any(e => e.Id == id);
        }
    }
}
