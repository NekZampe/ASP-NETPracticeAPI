using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebPractice.Models;

namespace WebPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            try
            {
                if (post == null)
                {
                    return BadRequest("Invalid post data.");
                }

                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();

                return Ok("Post created successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        // DELETE: api/posts/{postId}
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
                if (post == null)
                {
                    return NotFound("Post not found.");
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return Ok("Post deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        // PATCH: api/posts/like/{postId}
        [HttpPatch("{userId}/like/{postId}")]
        public async Task<IActionResult> ToggleLike(int postId, int userId)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
                if (post == null)
                {
                    return NotFound("Post not found.");
                }

               
                if (post.LikedBy == null)
                {
                    post.LikedBy = new int[] { userId };
                    post.LikeCount++;
                }
                else
                {
                    
                    if (post.LikedBy.Contains(userId))
                    {
                        post.LikedBy = post.LikedBy.Where(id => id != userId).ToArray();
                        post.LikeCount--;
                    }
                    else
                    {
                      
                        post.LikedBy = post.LikedBy.Append(userId).ToArray();
                        post.LikeCount++;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok("Like count updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _context.Posts.ToListAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
