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

        // GET: api/userfollowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFollower>>> GetUserFollowers()
        {
            return await _context.UserFollowers.ToListAsync();
        }

        //FOLLOW/UNFOLLOW

        [HttpPost("{userId}/follow/{followerId}")]
        public async Task<IActionResult> FollowUser(int userId, int followerId)
        {
            // Check if both user and follower exist
            var user = await _context.Users.FindAsync(userId);
            var follower = await _context.Users.FindAsync(followerId);


            if (userId == followerId) return Conflict("You can't follow yourself");

            if (follower == null && user == null)
            {
                return NotFound($"Users with ID {userId} , {followerId} not found.");
            }
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            if (follower == null)
            {
                return NotFound($"User with ID {followerId} not found.");
            }


            // Check if the relationship already exists
            var existingRelationship = await _context.UserFollowers
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FollowerId == followerId);

            if (existingRelationship != null)
            {
                return Conflict("Relationship already exists.");
            }

            var userFollower = new UserFollower
            {
                UserId = userId,
                FollowerId = followerId,
                User = user,
                Follower = follower
            };

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
