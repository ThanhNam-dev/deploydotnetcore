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
    [Route("friendship")]
    public class FriendShipController : Controller
    {
        private readonly DataDbContext _context;

        public FriendShipController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<FriendShip>>> GetAll()
        {
            return await _context.FriendShips.Where(p => p.IsActive == true && p.IsDeleted == false).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FriendShip>> Get(Guid id)
        {
            var quizType = await _context.FriendShips.FindAsync(id);

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpPost("")]
        public async Task<ActionResult<FriendShip>> Post(CreateFriendShipViewModel model)
        {
            //Methods Add Friend
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var childName = infoFromToken.Result.ChildName;
            FriendShip createItem = new FriendShip()
            {
                SenderId = Guid.Parse(userId),
                ReceipId = model.ReceipId,
                Confirmed = false,
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
            Notification createNoti = new Notification()
            {
                Id = Guid.NewGuid(),
                Content = childName + " đã gửi cho bạn một lời mời kết bạn!",
                UserId = model.ReceipId,
                Type = "addfriend",
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
            _context.FriendShips.Add(createItem);
            _context.Notifications.Add(createNoti);
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateFriendShipViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            
            FriendShip updateItem = new FriendShip()
            {
                Id = id,
                SenderId = model.SenderId,
                ReceipId = model.ReceipId,
                Confirmed = model.Confirmed,
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
        public async Task<ActionResult<FriendShip>> Delete(Guid id)
        {
            //Xóa nè
            var item = await _context.FriendShips.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.FriendShips.Remove(item);
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

            FriendShip updateItem = new FriendShip()
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
            return _context.FriendShips.Any(e => e.Id == id);
        }
    }
}
