using Microsoft.AspNetCore.Mvc;

namespace WebPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFollowerController : ControllerBase
    {
        private readonly DataContext _context;

        public UserFollowerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("userfollowers")]
        public async Task<IActionResult> GetUserFollowers()
        {
            var userFollowers = await _context.UserFollowers
                .Include(uf => uf.User)
                .Include(uf => uf.Follower)
                .ToListAsync();

            var result = userFollowers.Select(uf => new
            {
                uf.UserId,
                UserEmail = uf.User?.Email,
                uf.FollowerId,
                FollowerEmail = uf.Follower?.Email 
            });

            return Ok(result);
        }


        //FOLLOW/UNFOLLOW

        [HttpPost("{userId}/follow/{followerId}")]
        public async Task<IActionResult> FollowUser(int userId, int followerId)
        {
            
            var user = await _context.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var follower = await _context.Users
                .Include(u => u.Following)
                .FirstOrDefaultAsync(u => u.Id == followerId);

            if (user == null || follower == null)
            {
                return NotFound($"One or both users with IDs {userId}, {followerId} not found.");
            }

            
            var existingRelationship = await _context.UserFollowers
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FollowerId == followerId);

            if (existingRelationship != null)
            {
                return Conflict("Relationship already exists.");
            }

            var userFollower = new UserFollower
            {
                UserId = userId,
                FollowerId = followerId
            };

            user.Followers.Add(userFollower);
            follower.Following.Add(userFollower);

            _context.UserFollowers.Add(userFollower);
            await _context.SaveChangesAsync();

            return Ok($"User: {userId} successfully followed User: {followerId}");
        }




        [HttpDelete("{userId}/unfollow/{followerId}")]
        public async Task<IActionResult> UnfollowUser(int userId, int followerId)
        {
            var user = await _context.Users.FindAsync(userId);
            var follower = await _context.Users.FindAsync(followerId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            if (follower == null)
            {
                return NotFound($"User with ID {followerId} not found.");
            }

            var userFollower = await _context.UserFollowers.FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FollowerId == followerId);

            if (userFollower == null)
            {
                return NotFound("Relationship not found.");
            }

            _context.UserFollowers.Remove(userFollower);
            await _context.SaveChangesAsync();

            return Ok($"User with ID {userId} successfully unfollowed user with ID {followerId}");
        }


    }
}
