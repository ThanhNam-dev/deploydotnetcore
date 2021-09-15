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
    [Route("friends")]
    public class FriendListController : Controller
    {
        private readonly DataDbContext _context;

        public FriendListController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<FriendList>>> GetAll()
        {
            return await _context.FriendLists.Where(p => p.IsActive == true && p.IsDeleted == false).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FriendList>> Get(Guid id)
        {
            var quizType = await _context.FriendLists.FindAsync(id);

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpPost("")]
        public async Task<ActionResult<FriendList>> Post(CreateFriendListViewModel model)
        {
            //FriendId là id FriendShip
            // Methods Confirm Friend
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var childName = infoFromToken.Result.ChildName;
            var checkContext = await _context.FriendLists.Where(x=>x.FriendId == model.FriendId && x.UserId== Guid.Parse(userId)).ToListAsync();
            FriendList createItem = new FriendList()
            {
                FriendId = model.FriendId,
                UserId = Guid.Parse(userId),
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
            if (checkContext.Count == 0)
			{
                

                var quizType = await _context.FriendShips.FindAsync(model.FriendId);
                var users = await _context.AppUsers.FindAsync(quizType.SenderId);
                var updateShip = await _context.FriendShips.FindAsync(model.FriendId);
                updateShip.Confirmed = true;
                Notification createNoti = new Notification()
                {
                    Id = Guid.NewGuid(),
                    Content = childName + " đã chấp nhận lời mời kết bạn từ bạn!",
                    UserId = users.Id,
                    Type = "friend "+userId,
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
                FriendList createItem2 = new FriendList()
                {
                    FriendId = model.FriendId,
                    UserId = users.Id,
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
                _context.FriendShips.Update(updateShip);
                _context.FriendLists.Add(createItem);
                _context.FriendLists.Add(createItem2);
                _context.Notifications.Add(createNoti);
                await _context.SaveChangesAsync();
            }
            

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateFriendListViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            FriendList updateItem = new FriendList()
            {
                Id = id,
                FriendId = model.FriendId,
                UserId = model.UserId,
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
        public async Task<ActionResult<FriendList>> Delete(Guid id)
        {
            var item = await _context.FriendLists.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.FriendLists.Remove(item);
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

            FriendList updateItem = new FriendList()
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
            return _context.FriendLists.Any(e => e.Id == id);
        }
    }
}
