namespace WebPractice.Models
{
    public class Place
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; }
        public double Rating { get; set; }
        public string[] Reviews { get; set; }


    }

}