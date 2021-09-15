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
    [Route("lesson")]
    public class LessionController : Controller
    {
        private readonly DataDbContext _context;

        public LessionController(DataDbContext context)
        {
            _context = context;
        }

        // Get list lesson by quizType
        [HttpGet("GetLessonByType/{quizTypeId}")]
        public async Task<ActionResult<Lession>> GetLessonByType(Guid quizTypeId)
        {
            var quizType = await (from qt in _context.QuizTypes
                           join ls in _context.Lessions on qt.Id equals ls.QuizTypeId
                           select ls).ToListAsync();

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Lession>>> GetAll()
        {
            return await _context.Lessions.Where(p => p.IsActive == true && p.IsDeleted == false).ToListAsync();
        }

        [HttpGet("{idtype}")]
        public async Task<ActionResult<IEnumerable<Lession>>> Get(Guid idtype)
        {
            return await _context.Lessions.Where(p => p.IsActive == true && p.IsDeleted == false && p.QuizTypeId==idtype).ToListAsync();
        }

        [HttpPost("")]
        public async Task<ActionResult<Lession>> Post(CreateLessonViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

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
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateLessionViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Lession updateItem = new Lession()
            {
                Id = id,
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
        public async Task<ActionResult<Lession>> Delete(Guid id)
        {
            var item = await _context.Lessions.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Lessions.Remove(item);
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

            Lession updateItem = new Lession()
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
            return _context.Lessions.Any(e => e.Id == id);
        }
    }
}
