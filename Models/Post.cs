namespace WebPractice.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int LikeCount { get; set; }
        public int[] LikedBy { get; set; } 
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; } 
        public string[] ImageUrls { get; set; } = new string[3];
    }
}
