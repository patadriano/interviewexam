namespace MoviesAPI.Models
{
    public class Movie
    {
        public int movieID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isRented { get; set; }
        public DateTime? rentalDate { get; set; }
        public bool isDeleted { get; set; }
    }
}
