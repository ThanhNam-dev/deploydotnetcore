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
    [Route("users")]
    public class AppUserController : Controller
    {
        private readonly DataDbContext _context;

        public AppUserController(DataDbContext context)
        {
            _context = context;
        }
        [HttpGet("getId/{id}")]
        public async Task<ActionResult<AppUser>> getById(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            var quizType = await _context.AppUsers.FindAsync(id);

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAll()
        {
            return await _context.AppUsers.ToListAsync();
        }
        [HttpGet("getName/{name}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetName(string name)
        {
            //Methods Search Name
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var query = from s in _context.AppUsers where EF.Functions.Like(s.ChildName, "%" + name + "%") select s;
			if (name.Length==10)
			{
                var query2 = from s in _context.AppUsers where EF.Functions.Like(s.PhoneNumber, "%" + name + "%") select s;
                return await query2.ToListAsync();
            }
            return await query.Where(x=>x.Id!=Guid.Parse(userId)).ToListAsync();
        }

		[HttpGet("getUser")]
		public async Task<ActionResult<IEnumerable<AppUser>>> GetUser()
		{
            //Methods List Message
            string tokenString = Request.Headers["Authorization"].ToString();
			// Get UserId, ChildName, PhoneNumber from token
			var infoFromToken = Authorization.GetInfoFromToken(tokenString);
			var userId = infoFromToken.Result.UserId;
            List<FriendList> list = await _context.FriendLists.Where(x => x.UserId ==Guid.Parse(userId)).ToListAsync();
            List<FriendShip> friendShips = new List<FriendShip>();
            List<AppUser> appUsers = new List<AppUser>();
            foreach (var item in list)
            {
                FriendShip onefriendShip = await _context.FriendShips.Where(x => x.Id == item.FriendId).FirstOrDefaultAsync();
                friendShips.Add(onefriendShip);
            }
			foreach (var item in friendShips)
			{
				if (item.SenderId == Guid.Parse(userId))
				{
                    AppUser oneAppUser = await _context.AppUsers.Where(x => x.Id == item.ReceipId).FirstOrDefaultAsync();
                    appUsers.Add(oneAppUser);
                } else
				{
                    AppUser oneAppUser = await _context.AppUsers.Where(x => x.Id == item.SenderId).FirstOrDefaultAsync();
                    appUsers.Add(oneAppUser);
                }
            }
            return Ok(appUsers);
        }

		[HttpGet("profile")]
        public async Task<ActionResult<AppUser>> Profile()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            var quizType = await _context.AppUsers.FindAsync(Guid.Parse(userId));

            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }
        [HttpGet("listConfirm")]
        public async Task<ActionResult<IEnumerable<AppUser>>> ListConfirm()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            List<AppUser> appUsers = new List<AppUser>();
            var checkFriendShip = await _context.FriendShips.Where(x => x.ReceipId == Guid.Parse(userId) && x.Confirmed == false).ToListAsync();
            foreach (var item in checkFriendShip)
            {
                var oneItem = await _context.AppUsers.FindAsync(item.SenderId);
                oneItem.IdentityCard = item.Id.ToString();
                appUsers.Add(oneItem);
            }
            if (appUsers == null)
            {
                return NotFound();
            }

            return Ok(appUsers);
        }
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<AppUser>> ProfileIs(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            var quizType = await _context.AppUsers.FindAsync(id);
            var checkFriendShip = await _context.FriendShips.Where(x => x.SenderId == id && x.ReceipId == Guid.Parse(userId) && x.Confirmed == true).ToListAsync();
            var checkFriendShip2 = await _context.FriendShips.Where(x => x.ReceipId == id && x.SenderId == Guid.Parse(userId) && x.Confirmed == true).ToListAsync();
            if (checkFriendShip.Count == 1 || checkFriendShip2.Count == 1)
			{
                quizType.IdentityCard = "Đã kết bạn";
			} else
			{
                quizType.IdentityCard = "Chưa kết bạn";
            }
            var checkFriendShip3 = await _context.FriendShips.Where(x => x.SenderId == id && x.ReceipId == Guid.Parse(userId) && x.Confirmed == false).ToListAsync();
            var checkFriendShip4 = await _context.FriendShips.Where(x => x.ReceipId == id && x.SenderId == Guid.Parse(userId) && x.Confirmed == false).ToListAsync();
            if (checkFriendShip3.Count == 1 || checkFriendShip4.Count == 1)
            {
                quizType.AccessFailedCount = 2;
            }
            else
            {
                quizType.AccessFailedCount = 1;
            }
            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> PutUserInformation(UpdateProfileViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            AppUser updateItem = new AppUser()
            {
                Id = Guid.Parse(userId),
                ParentName = model.ParentName,
                ParentAge = model.ParentAge,
                ChildName = model.ChildName,
                ChildBirth = model.ChildBirth,
                ChildGender = model.ChildGender,
                IdentityCard = model.IdentityCard,
                Address = model.Address,
                UrlAvatar = model.UrlAvatar
            };
            _context.Entry(updateItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckExists(Guid.Parse(userId)))
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

        //[HttpDelete("hide/{id}")]
        //public async Task<IActionResult> Hide(Guid id)
        //{
        //    string tokenString = Request.Headers["Authorization"].ToString();
        //    // Get UserId, ChildName, PhoneNumber from token
        //    var infoFromToken = Authorization.GetInfoFromToken(tokenString);
        //    var userId = infoFromToken.Result.UserId;

        //    AppUser updateItem = new AppUser()
        //    {
        //        Id = id,
        //        DeletedBy = userId,
        //        IsActive = true,
        //        IsDeleted = true
        //    };
        //    _context.Entry(updateItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CheckExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok(updateItem);
        //}

        private bool CheckExists(Guid id)
        {
            return _context.AppUsers.Any(e => e.Id == id);
        }
    }
}
