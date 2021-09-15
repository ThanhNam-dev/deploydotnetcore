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
    [Route("quizType")]
    public class QuizTypeController : Controller
    {
        private readonly DataDbContext _context;

        public QuizTypeController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<QuizType>>> GetAll()
        {
            return await _context.QuizTypes.Where(p => p.IsActive == true && p.IsDeleted == false).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizType>> Get(Guid id)
        {
            var quizType = await _context.QuizTypes.FindAsync(id);

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpPost("")]
        public async Task<ActionResult<QuizType>> Post(CreateQuizTypeViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

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
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateQuizTypeViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            QuizType updateItem = new QuizType()
            {
                Id = id,
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
        public async Task<ActionResult<QuizType>> Delete(Guid id)
        {
            var item = await _context.QuizTypes.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.QuizTypes.Remove(item);
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

            QuizType updateItem = new QuizType()
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
            return _context.QuizTypes.Any(e => e.Id == id);
        }
    }
}
