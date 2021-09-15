using EMa.Common.Helpers;
using EMa.Data.DataContext;
using EMa.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMa.API.Controllers
{
    [ApiController]
    [Route("notifications")]
    public class NotificationController : Controller
    {
        
            private readonly DataDbContext _context;

            public NotificationController(DataDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            [Route("")]
            public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
            {
                string tokenString = Request.Headers["Authorization"].ToString();
                // Get UserId, ChildName, PhoneNumber from token
                var infoFromToken = Authorization.GetInfoFromToken(tokenString);
                var userId = infoFromToken.Result.UserId;
                return await _context.Notifications.Where(p => p.IsActive == true && p.IsDeleted == false && p.UserId==Guid.Parse(userId)).OrderByDescending(x=>x.CreatedDate).ToListAsync();
            }
            
        }
    
}
