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
    [Route("post")]
    public class PostController : Controller
    {
        private readonly DataDbContext _context;

        public PostController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            List<FriendList> list = await _context.FriendLists.Where(x => x.UserId == Guid.Parse(userId)).ToListAsync();
            List<FriendShip> friendShips = new List<FriendShip>();
            List<AppUser> appUsers = new List<AppUser>();
            List<Post> post = new List<Post>();
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
                }
                else
                {
                    AppUser oneAppUser = await _context.AppUsers.Where(x => x.Id == item.SenderId).FirstOrDefaultAsync();
                    appUsers.Add(oneAppUser);
                }
            }
            AppUser oneAppUsers = await _context.AppUsers.Where(x => x.Id == Guid.Parse(userId)).FirstOrDefaultAsync();
            appUsers.Add(oneAppUsers);
            foreach (var item in appUsers)
            {
                
                List<Post> lsPost = await _context.Posts.Where(x => x.UserId == item.Id).ToListAsync();
                foreach (var items in lsPost)
				{
                    List<PostComment> postComments = _context.PostComments.Where(x => x.PostId == items.Id).ToList();
                    items.DeletedBy = postComments.Count().ToString();
                    items.CreatedBy = item.ChildName;
					post.Add(items);
                }
                
			}

            return Ok(post.OrderByDescending(x=>x.CreatedDate));
        }

        [HttpGet("getPost")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            List<AppUser> appUsers = new List<AppUser>();
            List<Post> post = new List<Post>();


            AppUser oneAppUsers = await _context.AppUsers.Where(x => x.Id == Guid.Parse(userId)).FirstOrDefaultAsync();
            appUsers.Add(oneAppUsers);
            foreach (var item in appUsers)
            {
                List<Post> lsPost = await _context.Posts.Where(x => x.UserId == item.Id).ToListAsync();
                foreach (var items in lsPost)
                {
                    List<PostComment> postComments = await _context.PostComments.Where(x => x.PostId == items.Id).ToListAsync();
                    items.DeletedBy = postComments.Count().ToString();
                    items.CreatedBy = item.ChildName;
                    post.Add(items);
                }

            }

            return Ok(post);
        }

        [HttpGet("getPost/{id}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostUser(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            List<AppUser> appUsers = new List<AppUser>();
            List<Post> post = new List<Post>();

            
            AppUser oneAppUsers = await _context.AppUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            appUsers.Add(oneAppUsers);
            foreach (var item in appUsers)
            {
                List<Post> lsPost = await _context.Posts.Where(x => x.UserId == item.Id).ToListAsync();
                foreach (var items in lsPost)
                {
                    List<PostComment> postComments = await _context.PostComments.Where(x => x.PostId == items.Id).ToListAsync();
                    items.DeletedBy = postComments.Count().ToString();
                    items.CreatedBy = item.ChildName;
                    post.Add(items);
                }

            }

            return Ok(post);
        }
        [HttpGet("getPosts/{idpost}")]
        public async Task<ActionResult<Post>> GetPostOne(Guid idpost)
        {
            var quizType = await _context.Posts.FindAsync(idpost);
            var namePost = await _context.AppUsers.FindAsync(quizType.UserId);
            List<PostComment> postComments = await _context.PostComments.Where(x => x.PostId == idpost).ToListAsync();
            quizType.DeletedBy = postComments.Count().ToString();
            quizType.CreatedBy = namePost.ChildName;
            if (quizType == null)
            {
                return NotFound();
            }

            return Ok(quizType);
        }

        [HttpPost("")]
        public async Task<ActionResult<Post>> Post(CreatePostViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Post createItem = new Post()
            {
                UserId = Guid.Parse(userId),
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

            _context.Posts.Add(createItem);
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdatePostViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            Post updateItem = new Post()
            {
                Id = id,
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
        public async Task<ActionResult<Post>> Delete(Guid id)
        {
            var item = await _context.Posts.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(item);
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

            Post updateItem = new Post()
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
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
