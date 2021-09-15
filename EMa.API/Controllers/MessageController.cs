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
	[Route("messages")]
	public class MessageController : Controller
	{
        private readonly DataDbContext _context;

        public MessageController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetId(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            List<Message> messages = new List<Message>();
            //Error
            List<Message> sender = await _context.Messages.Where(p =>  p.IsDeleted == false && p.CreatedBy==userId && p.UserId == id).ToListAsync();
            List<Message> reply = await _context.Messages.Where(p => p.IsDeleted == false && p.CreatedBy == id.ToString() && p.UserId == Guid.Parse(userId)).ToListAsync();
			foreach (var item in sender)
			{
                item.IsActive = true;
                messages.Add(item);
			}
            foreach (var item in reply)
            {
                item.IsActive = false;
                messages.Add(item);
            }
            return Ok(messages.OrderBy(x=>x.CreatedDate));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Message>> Post(CreateMessageViewModel model,Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Message createItem = new Message()
            {
                UserId = id,
                Content = model.Content,
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

            _context.Messages.Add(createItem);
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }
    }
}
