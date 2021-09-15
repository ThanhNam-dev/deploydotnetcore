using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Common.Helpers;
using EMa.Data.ViewModel;

namespace EMa.API.Controllers
{
    [Route("postcomment")]
    [ApiController]
    public class PostCommentController : Controller
    {
        private readonly DataDbContext _context;

        public PostCommentController(DataDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<PostComment>>> GetId(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            List<PostComment> comments = await _context.PostComments.Where(p => p.PostId == id).ToListAsync();
            List<PostComment> renderComment = new List<PostComment>();
            
            foreach (var item in comments)
            {
                var user = await _context.AppUsers.FindAsync(item.UserId);
                item.CreatedBy = user.ChildName;
                renderComment.Add(item);
            }
            
            return Ok(comments.OrderBy(x => x.CreatedDate));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<PostComment>> Post(CreatePostCommentViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            // Get UserId, ChildName, PhoneNumber from token
            var infoFromToken = Authorization.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var childName = infoFromToken.Result.ChildName;
            PostComment createItem = new PostComment()
            {
                PostId = id,
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
            Post checkPost = await _context.Posts.Where(x=>x.Id==id).FirstOrDefaultAsync();
			if (checkPost.UserId == Guid.Parse(userId))
			{
                List<Guid> IdUser = new List<Guid>();
                List<PostComment> checkComment = await _context.PostComments.Where(x => x.PostId == id).ToListAsync();
                int caches;
				foreach (var items in checkComment)
				{
                    caches = 0;
					if (items.UserId != Guid.Parse(userId))
					{
                        
						foreach (var userItem in IdUser)
						{
							if (userItem == items.UserId)
							{
                                caches = 1;
							}
						}
						if (caches == 0)
						{
                            IdUser.Add(items.UserId);
                        }
					}
				}
				foreach (var item in IdUser)
				{
                    Notification createNoti = new Notification()
                    {
                        Id = Guid.NewGuid(),
                        Content = childName + " cũng đã bình luận bài viết của họ!",
                        UserId = item,
                        Type = "post " + id.ToString(),
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
                    _context.Notifications.Add(createNoti);
                }
			} else
			{
                Notification createNoti = new Notification()
                {
                    Id = Guid.NewGuid(),
                    Content = childName + " đã bình luận vào bài viết của bạn!",
                    UserId = checkPost.UserId,
                    Type = "post "+id.ToString(),
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
                _context.Notifications.Add(createNoti);
            }
            
            
            _context.PostComments.Add(createItem);
            await _context.SaveChangesAsync();

            return Ok(createItem);
        }
    }
}
